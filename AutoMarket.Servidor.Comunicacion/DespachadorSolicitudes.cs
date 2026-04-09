/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase encargada de interpretar y despachar las solicitudes recibidas por TCP hacia la lógica de negocio correspondiente.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-06
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Logica;

namespace AutoMarket.Servidor.Comunicacion
{
    public sealed class DespachadorSolicitudes
    {
        private readonly AutenticacionClienteLogica _autenticacionClienteLogica;
        private readonly SucursalLogica _sucursalLogica;
        private readonly VehiculoxSucursalLogica _vehiculoxSucursalLogica;
        private readonly VentaLogica _ventaLogica;
        private readonly ClienteLogica _clienteLogica;
        private readonly VehiculoLogica _vehiculoLogica;

        public DespachadorSolicitudes()
        {
            _autenticacionClienteLogica = new AutenticacionClienteLogica();
            _sucursalLogica = new SucursalLogica();
            _vehiculoxSucursalLogica = new VehiculoxSucursalLogica();
            _ventaLogica = new VentaLogica();
            _clienteLogica = new ClienteLogica();
            _vehiculoLogica = new VehiculoLogica();
        }

        public DespachadorSolicitudes(
            AutenticacionClienteLogica autenticacionClienteLogica,
            SucursalLogica sucursalLogica,
            VehiculoxSucursalLogica vehiculoxSucursalLogica,
            VentaLogica ventaLogica,
            ClienteLogica clienteLogica,
            VehiculoLogica vehiculoLogica)
        {
            _autenticacionClienteLogica = autenticacionClienteLogica ?? throw new ArgumentNullException(nameof(autenticacionClienteLogica));
            _sucursalLogica = sucursalLogica ?? throw new ArgumentNullException(nameof(sucursalLogica));
            _vehiculoxSucursalLogica = vehiculoxSucursalLogica ?? throw new ArgumentNullException(nameof(vehiculoxSucursalLogica));
            _ventaLogica = ventaLogica ?? throw new ArgumentNullException(nameof(ventaLogica));
            _clienteLogica = clienteLogica ?? throw new ArgumentNullException(nameof(clienteLogica));
            _vehiculoLogica = vehiculoLogica ?? throw new ArgumentNullException(nameof(vehiculoLogica));
        }

        public string ProcesarSolicitud(string solicitud)
        {
            string solicitudNormalizada = solicitud?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(solicitudNormalizada))
            {
                throw new ArgumentException("La solicitud recibida está vacía.");
            }

            string[] partes = solicitudNormalizada.Split('|');
            string comando = (partes[0] ?? string.Empty).Trim().ToUpperInvariant();

            return comando switch
            {
                "PING" => ProcesarPing(),
                "AUTENTICAR_CLIENTE" => ProcesarAutenticacionCliente(partes),
                "VALIDAR_CLIENTE" => ProcesarAutenticacionCliente(partes),
                "OBTENER_SUCURSALES_ACTIVAS" => ProcesarObtenerSucursalesActivas(),
                "OBTENER_VEHICULOS_POR_SUCURSAL" => ProcesarObtenerVehiculosPorSucursal(partes),
                "REGISTRAR_VENTA" => ProcesarRegistrarVenta(partes),
                "OBTENER_VENTAS_POR_CLIENTE" => ProcesarObtenerVentasPorCliente(partes),
                "OBTENER_CLIENTE_POR_ID" => ProcesarObtenerClientePorId(partes),
                "OBTENER_VEHICULO_POR_ID" => ProcesarObtenerVehiculoPorId(partes),
                _ => throw new ArgumentException("El comando solicitado no es reconocido por el servidor.")
            };
        }

        private string ProcesarPing()
        {
            return "OK|PONG";
        }

        private string ProcesarAutenticacionCliente(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud de autenticación requiere la identificación del cliente.");

            string identificacion = ObtenerTextoRequerido(partes[1], "La identificación del cliente es obligatoria.");

            Cliente cliente = _autenticacionClienteLogica.AutenticarPorIdentificacion(identificacion);

            return string.Join("|",
                "OK",
                "CLIENTE_AUTENTICADO",
                cliente.IdCliente.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(cliente.Identificacion),
                EscaparCampo(cliente.NombreCompleto),
                cliente.Activo ? "1" : "0");
        }

        private string ProcesarObtenerSucursalesActivas()
        {
            List<Sucursal> sucursales = _sucursalLogica.ObtenerActivas();

            StringBuilder respuesta = new StringBuilder();
            respuesta.Append("OK|SUCURSALES_ACTIVAS|");
            respuesta.Append(sucursales.Count.ToString(CultureInfo.InvariantCulture));

            foreach (Sucursal sucursal in sucursales)
            {
                respuesta.Append("|");
                respuesta.Append(FormatearSucursal(sucursal));
            }

            return respuesta.ToString();
        }

        private string ProcesarObtenerVehiculosPorSucursal(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud requiere el id de la sucursal.");

            int idSucursal = ObtenerEnteroRequerido(partes[1], "El id de la sucursal no es válido.");

            List<VehiculoxSucursal> inventario = _vehiculoxSucursalLogica.ObtenerPorSucursal(idSucursal);

            StringBuilder respuesta = new StringBuilder();
            respuesta.Append("OK|VEHICULOS_POR_SUCURSAL|");
            respuesta.Append(inventario.Count.ToString(CultureInfo.InvariantCulture));

            foreach (VehiculoxSucursal item in inventario)
            {
                respuesta.Append("|");
                respuesta.Append(FormatearVehiculoxSucursal(item));
            }

            return respuesta.ToString();
        }

        private string ProcesarRegistrarVenta(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 4, "La solicitud de registro de venta requiere idCliente, idSucursal e idVehiculo.");

            int idCliente = ObtenerEnteroRequerido(partes[1], "El id del cliente no es válido.");
            int idSucursal = ObtenerEnteroRequerido(partes[2], "El id de la sucursal no es válido.");
            int idVehiculo = ObtenerEnteroRequerido(partes[3], "El id del vehículo no es válido.");

            Cliente? cliente = _clienteLogica.ObtenerPorId(idCliente);
            if (cliente == null)
            {
                throw new InvalidOperationException("El cliente indicado no existe.");
            }

            Sucursal? sucursal = _sucursalLogica.ObtenerPorId(idSucursal);
            if (sucursal == null)
            {
                throw new InvalidOperationException("La sucursal indicada no existe.");
            }

            Vehiculo? vehiculo = _vehiculoLogica.ObtenerPorId(idVehiculo);
            if (vehiculo == null)
            {
                throw new InvalidOperationException("El vehículo indicado no existe.");
            }

            Venta venta = new Venta(
                cliente,
                sucursal,
                vehiculo,
                DateTime.Now,
                vehiculo.Precio);

            int idVentaGenerado = _ventaLogica.RegistrarVenta(venta);

            return string.Join("|",
                "OK",
                "VENTA_REGISTRADA",
                idVentaGenerado.ToString(CultureInfo.InvariantCulture),
                idCliente.ToString(CultureInfo.InvariantCulture),
                idSucursal.ToString(CultureInfo.InvariantCulture),
                idVehiculo.ToString(CultureInfo.InvariantCulture),
                vehiculo.Precio.ToString(CultureInfo.InvariantCulture));
        }

        private string ProcesarObtenerVentasPorCliente(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud requiere el id del cliente.");

            int idCliente = ObtenerEnteroRequerido(partes[1], "El id del cliente no es válido.");

            List<Venta> ventas = _ventaLogica.ObtenerPorCliente(idCliente);

            StringBuilder respuesta = new StringBuilder();
            respuesta.Append("OK|VENTAS_POR_CLIENTE|");
            respuesta.Append(ventas.Count.ToString(CultureInfo.InvariantCulture));

            foreach (Venta venta in ventas)
            {
                respuesta.Append("|");
                respuesta.Append(FormatearVenta(venta));
            }

            return respuesta.ToString();
        }

        private string ProcesarObtenerClientePorId(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud requiere el id del cliente.");

            int idCliente = ObtenerEnteroRequerido(partes[1], "El id del cliente no es válido.");

            Cliente? cliente = _clienteLogica.ObtenerPorId(idCliente);
            if (cliente == null)
            {
                throw new InvalidOperationException("No existe un cliente registrado con el id indicado.");
            }

            return string.Join("|",
                "OK",
                "CLIENTE",
                cliente.IdCliente.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(cliente.Identificacion),
                EscaparCampo(cliente.NombreCompleto),
                cliente.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                cliente.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                cliente.Activo ? "1" : "0");
        }

        private string ProcesarObtenerVehiculoPorId(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud requiere el id del vehículo.");

            int idVehiculo = ObtenerEnteroRequerido(partes[1], "El id del vehículo no es válido.");

            Vehiculo? vehiculo = _vehiculoLogica.ObtenerPorId(idVehiculo);
            if (vehiculo == null)
            {
                throw new InvalidOperationException("No existe un vehículo registrado con el id indicado.");
            }

            return string.Join("|",
                "OK",
                "VEHICULO",
                vehiculo.IdVehiculo.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Marca),
                EscaparCampo(vehiculo.Modelo),
                vehiculo.Anio.ToString(CultureInfo.InvariantCulture),
                vehiculo.Precio.ToString(CultureInfo.InvariantCulture),
                vehiculo.Estado.ToString(),
                vehiculo.Categoria.IdCategoria.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Categoria.NombreCategoria),
                EscaparCampo(vehiculo.Categoria.Descripcion));
        }

        private void ValidarCantidadMinimaPartes(string[] partes, int cantidadMinima, string mensajeError)
        {
            if (partes == null || partes.Length < cantidadMinima)
            {
                throw new ArgumentException(mensajeError);
            }
        }

        private string ObtenerTextoRequerido(string valor, string mensajeError)
        {
            string texto = valor?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                throw new ArgumentException(mensajeError);
            }

            return texto;
        }

        private int ObtenerEnteroRequerido(string valor, string mensajeError)
        {
            string texto = valor?.Trim() ?? string.Empty;

            if (!int.TryParse(texto, NumberStyles.Integer, CultureInfo.InvariantCulture, out int resultado) || resultado <= 0)
            {
                throw new ArgumentException(mensajeError);
            }

            return resultado;
        }

        private string FormatearSucursal(Sucursal sucursal)
        {
            return string.Join("^",
                sucursal.IdSucursal.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(sucursal.Nombre),
                EscaparCampo(sucursal.Direccion),
                EscaparCampo(sucursal.Telefono),
                sucursal.VendedorEncargado != null ? sucursal.VendedorEncargado.IdVendedor.ToString(CultureInfo.InvariantCulture) : "0",
                sucursal.VendedorEncargado != null ? EscaparCampo(sucursal.VendedorEncargado.NombreCompleto) : string.Empty,
                sucursal.VendedorEncargado != null ? EscaparCampo(sucursal.VendedorEncargado.Identificacion) : string.Empty,
                sucursal.Activo ? "1" : "0");
        }

        private string FormatearVehiculoxSucursal(VehiculoxSucursal vehiculoxSucursal)
        {
            Vehiculo vehiculo = vehiculoxSucursal.Vehiculo;

            return string.Join("^",
                vehiculoxSucursal.Sucursal.IdSucursal.ToString(CultureInfo.InvariantCulture),
                vehiculo.IdVehiculo.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Marca),
                EscaparCampo(vehiculo.Modelo),
                vehiculo.Anio.ToString(CultureInfo.InvariantCulture),
                vehiculo.Precio.ToString(CultureInfo.InvariantCulture),
                vehiculo.Estado.ToString(),
                EscaparCampo(vehiculo.EstadoDescripcion),
                vehiculo.Categoria.IdCategoria.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Categoria.NombreCategoria),
                EscaparCampo(vehiculo.Categoria.Descripcion),
                vehiculoxSucursal.Cantidad.ToString(CultureInfo.InvariantCulture));
        }

        private string FormatearVenta(Venta venta)
        {
            return string.Join("^",
                venta.IdVenta.ToString(CultureInfo.InvariantCulture),
                venta.Cliente.IdCliente.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Cliente.NombreCompleto),
                EscaparCampo(venta.Cliente.Identificacion),
                venta.Sucursal.IdSucursal.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Sucursal.Nombre),
                venta.Vehiculo.IdVehiculo.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Vehiculo.Marca),
                EscaparCampo(venta.Vehiculo.Modelo),
                venta.Vehiculo.Anio.ToString(CultureInfo.InvariantCulture),
                venta.Vehiculo.Precio.ToString(CultureInfo.InvariantCulture),
                venta.Vehiculo.Estado.ToString(),
                EscaparCampo(venta.Vehiculo.EstadoDescripcion),
                venta.Vehiculo.Categoria.IdCategoria.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Vehiculo.Categoria.NombreCategoria),
                EscaparCampo(venta.Vehiculo.Categoria.Descripcion),
                venta.FechaVenta.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                venta.Monto.ToString(CultureInfo.InvariantCulture));
        }

        private string EscaparCampo(string valor)
        {
            string texto = valor?.Trim() ?? string.Empty;

            texto = texto.Replace("^", "/");
            texto = texto.Replace("|", "/");
            texto = texto.Replace("\r", " ");
            texto = texto.Replace("\n", " ");

            return texto;
        }
    }
}