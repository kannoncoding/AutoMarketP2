/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase encargada de administrar la comunicación TCP de un cliente conectado, procesando solicitudes, devolviendo respuestas y notificando eventos del sistema.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-06
*/

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AutoMarket.Servidor.Comunicacion
{
    public sealed class ManejadorClienteTcp
    {
        private readonly TcpClient _clienteTcp;
        private readonly DespachadorSolicitudes _despachadorSolicitudes;
        private readonly object _bloqueoEnvio;

        public event Action<string>? EventoComunicacion;
        public event Action<TcpClient>? ClienteDesconectado;

        public TcpClient ClienteTcp
        {
            get => _clienteTcp;
        }

        public bool ClienteConectado
        {
            get
            {
                try
                {
                    return _clienteTcp.Connected;
                }
                catch
                {
                    return false;
                }
            }
        }

        public ManejadorClienteTcp(TcpClient clienteTcp)
        {
            _clienteTcp = clienteTcp ?? throw new ArgumentNullException(nameof(clienteTcp), "El cliente TCP es obligatorio.");
            _despachadorSolicitudes = new DespachadorSolicitudes();
            _bloqueoEnvio = new object();
        }

        public ManejadorClienteTcp(TcpClient clienteTcp, DespachadorSolicitudes despachadorSolicitudes)
        {
            _clienteTcp = clienteTcp ?? throw new ArgumentNullException(nameof(clienteTcp), "El cliente TCP es obligatorio.");
            _despachadorSolicitudes = despachadorSolicitudes ?? throw new ArgumentNullException(nameof(despachadorSolicitudes), "El despachador de solicitudes es obligatorio.");
            _bloqueoEnvio = new object();
        }

        public void ProcesarCliente()
        {
            string direccionRemota = ObtenerDireccionRemota();

            NotificarEvento("Inicio de procesamiento para cliente " + direccionRemota + ".");

            NetworkStream? flujoRed = null;
            StreamReader? lector = null;
            StreamWriter? escritor = null;

            try
            {
                flujoRed = _clienteTcp.GetStream();
                lector = new StreamReader(flujoRed, Encoding.UTF8, false, 1024, true);
                escritor = new StreamWriter(flujoRed, Encoding.UTF8, 1024, true)
                {
                    AutoFlush = true
                };

                EnviarRespuesta(escritor, "OK|CONEXION_ESTABLECIDA");

                while (ClienteConectado)
                {
                    string? solicitudRecibida = lector.ReadLine();

                    if (solicitudRecibida == null)
                    {
                        NotificarEvento("El cliente " + direccionRemota + " cerró la conexión.");
                        break;
                    }

                    string solicitudNormalizada = solicitudRecibida.Trim();

                    if (string.IsNullOrWhiteSpace(solicitudNormalizada))
                    {
                        EnviarRespuesta(escritor, "ERROR|SOLICITUD_VACIA");
                        continue;
                    }

                    NotificarEvento("Solicitud recibida desde " + direccionRemota + ": " + solicitudNormalizada);

                    if (EsSolicitudDeDesconexion(solicitudNormalizada))
                    {
                        EnviarRespuesta(escritor, "OK|DESCONEXION_ACEPTADA");
                        NotificarEvento("El cliente " + direccionRemota + " solicitó finalizar la conexión.");
                        break;
                    }

                    string respuesta = ProcesarSolicitudSegura(solicitudNormalizada);

                    EnviarRespuesta(escritor, respuesta);

                    NotificarEvento("Respuesta enviada a " + direccionRemota + ": " + respuesta);
                }
            }
            catch (IOException ex)
            {
                NotificarEvento("Error de entrada/salida con el cliente " + direccionRemota + ": " + ex.Message);
            }
            catch (SocketException ex)
            {
                NotificarEvento("Error de socket con el cliente " + direccionRemota + ": " + ex.Message);
            }
            catch (ObjectDisposedException)
            {
                NotificarEvento("La conexión del cliente " + direccionRemota + " fue liberada durante el procesamiento.");
            }
            catch (Exception ex)
            {
                NotificarEvento("Error inesperado durante el procesamiento del cliente " + direccionRemota + ": " + ex.Message);
            }
            finally
            {
                try
                {
                    escritor?.Dispose();
                }
                catch
                {
                }

                try
                {
                    lector?.Dispose();
                }
                catch
                {
                }

                try
                {
                    flujoRed?.Dispose();
                }
                catch
                {
                }

                try
                {
                    _clienteTcp.Close();
                }
                catch
                {
                }

                ClienteDesconectado?.Invoke(_clienteTcp);
                NotificarEvento("Finalizó el procesamiento del cliente " + direccionRemota + ".");
            }
        }

        private string ProcesarSolicitudSegura(string solicitud)
        {
            try
            {
                string respuesta = _despachadorSolicitudes.ProcesarSolicitud(solicitud);

                if (string.IsNullOrWhiteSpace(respuesta))
                {
                    return "ERROR|RESPUESTA_NO_GENERADA";
                }

                return respuesta;
            }
            catch (ArgumentException ex)
            {
                return "ERROR|SOLICITUD_INVALIDA|" + LimpiarTextoParaTransmision(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return "ERROR|OPERACION_INVALIDA|" + LimpiarTextoParaTransmision(ex.Message);
            }
            catch (Exception ex)
            {
                return "ERROR|ERROR_INTERNO|" + LimpiarTextoParaTransmision(ex.Message);
            }
        }

        private void EnviarRespuesta(StreamWriter escritor, string respuesta)
        {
            if (escritor == null)
            {
                throw new ArgumentNullException(nameof(escritor), "El escritor de respuesta es obligatorio.");
            }

            string respuestaNormalizada = respuesta?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(respuestaNormalizada))
            {
                respuestaNormalizada = "ERROR|RESPUESTA_VACIA";
            }

            lock (_bloqueoEnvio)
            {
                escritor.WriteLine(respuestaNormalizada);
                escritor.Flush();
            }
        }

        private bool EsSolicitudDeDesconexion(string solicitud)
        {
            string textoNormalizado = solicitud?.Trim().ToUpperInvariant() ?? string.Empty;

            return textoNormalizado == "SALIR"
                || textoNormalizado == "DESCONECTAR"
                || textoNormalizado == "QUIT"
                || textoNormalizado == "EXIT";
        }

        private string ObtenerDireccionRemota()
        {
            try
            {
                if (_clienteTcp.Client.RemoteEndPoint != null)
                {
                    return _clienteTcp.Client.RemoteEndPoint.ToString() ?? "Cliente desconocido";
                }
            }
            catch
            {
            }

            return "Cliente desconocido";
        }

        private string LimpiarTextoParaTransmision(string texto)
        {
            string textoNormalizado = texto?.Trim() ?? string.Empty;

            textoNormalizado = textoNormalizado.Replace("\r", " ");
            textoNormalizado = textoNormalizado.Replace("\n", " ");
            textoNormalizado = textoNormalizado.Replace("|", "/");

            return textoNormalizado;
        }

        private void NotificarEvento(string mensaje)
        {
            string mensajeConFecha = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " + mensaje;
            EventoComunicacion?.Invoke(mensajeConFecha);
        }
    }
}