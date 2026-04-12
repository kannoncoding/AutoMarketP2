/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase encargada de iniciar, administrar y detener el servidor TCP de AutoMarket, aceptando múltiples clientes concurrentes y notificando eventos del sistema.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-11
*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AutoMarket.Servidor.Comunicacion
{
    public sealed class ServidorTcp
    {
        private readonly IPAddress _direccionIp;
        private readonly int _puerto;
        private readonly object _bloqueoClientes;
        private readonly object _bloqueoEstado;
        private readonly List<TcpClient> _clientesConectados;

        private TcpListener? _tcpListener;
        private Thread? _hiloEscucha;
        private bool _estaEnEjecucion;

        public event Action<string>? EventoServidor;

        public IPAddress DireccionIp
        {
            get => _direccionIp;
        }

        public int Puerto
        {
            get => _puerto;
        }

        public bool EstaEnEjecucion
        {
            get
            {
                lock (_bloqueoEstado)
                {
                    return _estaEnEjecucion;
                }
            }
        }

        public int CantidadClientesConectados
        {
            get
            {
                lock (_bloqueoClientes)
                {
                    return _clientesConectados.Count;
                }
            }
        }

        public ServidorTcp()
            : this("127.0.0.1", 15500)
        {
        }

        public ServidorTcp(string direccionIp, int puerto)
        {
            if (string.IsNullOrWhiteSpace(direccionIp))
            {
                throw new ArgumentException("La dirección IP del servidor es obligatoria.", nameof(direccionIp));
            }

            if (!IPAddress.TryParse(direccionIp.Trim(), out IPAddress? direccionParseada))
            {
                throw new ArgumentException("La dirección IP indicada no es válida.", nameof(direccionIp));
            }

            if (puerto <= 0 || puerto > 65535)
            {
                throw new ArgumentException("El puerto del servidor no es válido.", nameof(puerto));
            }

            _direccionIp = direccionParseada;
            _puerto = puerto;
            _bloqueoClientes = new object();
            _bloqueoEstado = new object();
            _clientesConectados = new List<TcpClient>();
        }

        public void Iniciar()
        {
            lock (_bloqueoEstado)
            {
                if (_estaEnEjecucion)
                {
                    throw new InvalidOperationException("El servidor TCP ya se encuentra en ejecución.");
                }

                _tcpListener = new TcpListener(_direccionIp, _puerto);

                try
                {
                    _tcpListener.Start();
                }
                catch (SocketException ex)
                {
                    _tcpListener = null;
                    throw new InvalidOperationException("No fue posible iniciar el servidor TCP en la dirección y puerto configurados.", ex);
                }

                _estaEnEjecucion = true;

                _hiloEscucha = new Thread(EscucharClientes)
                {
                    IsBackground = true,
                    Name = "HiloEscuchaServidorTcp"
                };

                _hiloEscucha.Start();
            }

            NotificarEvento("Servidor TCP iniciado en " + _direccionIp + ":" + _puerto + ".");
        }

        public void Detener()
        {
            bool estabaEnEjecucion;

            lock (_bloqueoEstado)
            {
                estabaEnEjecucion = _estaEnEjecucion;
                _estaEnEjecucion = false;
            }

            if (!estabaEnEjecucion)
            {
                return;
            }

            try
            {
                _tcpListener?.Stop();
            }
            catch
            {
            }

            CerrarClientesConectados();

            try
            {
                if (_hiloEscucha != null && _hiloEscucha.IsAlive)
                {
                    _hiloEscucha.Join(1000);
                }
            }
            catch
            {
            }

            _tcpListener = null;
            _hiloEscucha = null;

            NotificarEvento("Servidor TCP detenido.");
        }

        private void EscucharClientes()
        {
            TcpListener? listenerLocal = _tcpListener;

            if (listenerLocal == null)
            {
                NotificarEvento("No se pudo iniciar la escucha de clientes porque el listener TCP no está inicializado.");
                return;
            }

            while (EstaEnEjecucion)
            {
                TcpClient? cliente = null;

                try
                {
                    cliente = listenerLocal.AcceptTcpClient();

                    lock (_bloqueoClientes)
                    {
                        _clientesConectados.Add(cliente);
                    }

                    string direccionRemota = ObtenerDireccionRemota(cliente);
                    NotificarEvento("Cliente conectado desde " + direccionRemota + ".");

                    ManejadorClienteTcp manejadorCliente = new ManejadorClienteTcp(cliente);
                    manejadorCliente.EventoComunicacion += ManejarEventoCliente;
                    manejadorCliente.ClienteDesconectado += ManejarClienteDesconectado;

                    Thread hiloCliente = new Thread(manejadorCliente.ProcesarCliente)
                    {
                        IsBackground = true,
                        Name = "HiloClienteTcp_" + NormalizarTextoParaNombreHilo(direccionRemota)
                    };

                    hiloCliente.Start();
                }
                catch (SocketException ex)
                {
                    if (EstaEnEjecucion)
                    {
                        NotificarEvento("Error de socket al aceptar un cliente: " + ex.Message);
                    }
                }
                catch (ObjectDisposedException)
                {
                    if (EstaEnEjecucion)
                    {
                        NotificarEvento("El listener TCP fue liberado mientras el servidor seguía en ejecución.");
                    }
                }
                catch (Exception ex)
                {
                    NotificarEvento("Ocurrió un error inesperado al aceptar un cliente: " + ex.Message);

                    if (cliente != null)
                    {
                        try
                        {
                            lock (_bloqueoClientes)
                            {
                                _clientesConectados.Remove(cliente);
                            }

                            cliente.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private void ManejarEventoCliente(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
            {
                return;
            }

            NotificarEvento(mensaje);
        }

        private void ManejarClienteDesconectado(TcpClient cliente)
        {
            if (cliente == null)
            {
                return;
            }

            string direccionRemota = ObtenerDireccionRemota(cliente);

            lock (_bloqueoClientes)
            {
                _clientesConectados.Remove(cliente);
            }

            try
            {
                cliente.Close();
            }
            catch
            {
            }

            NotificarEvento("Cliente desconectado: " + direccionRemota + ".");
        }

        private void CerrarClientesConectados()
        {
            List<TcpClient> clientesParaCerrar;

            lock (_bloqueoClientes)
            {
                clientesParaCerrar = new List<TcpClient>(_clientesConectados);
                _clientesConectados.Clear();
            }

            foreach (TcpClient cliente in clientesParaCerrar)
            {
                try
                {
                    cliente.Close();
                }
                catch
                {
                }
            }
        }

        private string ObtenerDireccionRemota(TcpClient cliente)
        {
            if (cliente == null)
            {
                return "Cliente desconocido";
            }

            try
            {
                if (cliente.Client.RemoteEndPoint != null)
                {
                    return cliente.Client.RemoteEndPoint.ToString() ?? "Cliente desconocido";
                }
            }
            catch
            {
            }

            return "Cliente desconocido";
        }

        private string NormalizarTextoParaNombreHilo(string texto)
        {
            string textoNormalizado = texto?.Trim() ?? "Cliente";

            textoNormalizado = textoNormalizado.Replace(":", "_");
            textoNormalizado = textoNormalizado.Replace(".", "_");
            textoNormalizado = textoNormalizado.Replace("/", "_");
            textoNormalizado = textoNormalizado.Replace("\\", "_");
            textoNormalizado = textoNormalizado.Replace(" ", "_");

            if (string.IsNullOrWhiteSpace(textoNormalizado))
            {
                return "Cliente";
            }

            return textoNormalizado;
        }

        private void NotificarEvento(string mensaje)
        {
            string mensajeConFecha = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " + mensaje;
            EventoServidor?.Invoke(mensajeConFecha);
        }
    }
}