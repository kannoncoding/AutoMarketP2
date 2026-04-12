/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #1
Descripción: Clase encargada de interpretar solicitudes TCP del cliente y generar respuestas protocolarias del servidor.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-06
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Logica;

namespace AutoMarket.Servidor.Comunicacion
{
    public sealed class DespachadorSolicitudes
    {
        private readonly SucursalLogica _sucursalLogica;
        private readonly VehiculoxSucursalLogica _vehiculoxSucursalLogica;
        private readonly VentaLogica _ventaLogica;
        private readonly ClienteLogica _clienteLogica;
        private readonly VehiculoLogica _vehiculoLogica;

        public DespachadorSolicitudes()
        {
            _sucursalLogica = new SucursalLogica();
            _vehiculoxSucursalLogica = new VehiculoxSucursalLogica();
            _ventaLogica = new VentaLogica();
            _clienteLogica = new ClienteLogica();
            _vehiculoLogica = new VehiculoLogica();
        }

        public DespachadorSolicitudes(
            SucursalLogica sucursalLogica,
            VehiculoxSucursalLogica vehiculoxSucursalLogica,
            VentaLogica ventaLogica,
            ClienteLogica clienteLogica,
            VehiculoLogica vehiculoLogica)
        {
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
                "LOGIN" => ProcesarLogin(partes),

                "SUCURSALES_ACTIVAS" => ProcesarObtenerSucursalesActivas(),
                "VEHICULOS_POR_SUCURSAL" => ProcesarObtenerVehiculosPorSucursal(partes),
                "VENTA" => ProcesarRegistrarVenta(partes),
                "VENTAS_POR_CLIENTE" => ProcesarObtenerVentasPorCliente(partes),

                "CLIENTE_POR_ID" => ProcesarObtenerClientePorId(partes),
                "VEHICULO_POR_ID" => ProcesarObtenerVehiculoPorId(partes),

                "AUTENTICAR_CLIENTE" => ProcesarLoginAlias(partes),
                "VALIDAR_CLIENTE" => ProcesarLoginAlias(partes),
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
            return "OK|PING|PONG";
        }

        private string ProcesarLogin(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud LOGIN requiere el id del cliente.");

            int idCliente = ObtenerEnteroRequerido(partes[1], "El id del cliente no es válido.");

            Cliente? cliente = _clienteLogica.ObtenerPorId(idCliente);
            if (cliente == null || !cliente.Activo)
            {
                throw new InvalidOperationException("El cliente no existe o no se encuentra activo.");
            }

            return string.Join("|",
                "OK",
                "LOGIN",
                cliente.IdCliente.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(cliente.NombreCompleto));
        }

        private string ProcesarLoginAlias(string[] partes)
        {
            return ProcesarLogin(partes);
        }

        private string ProcesarObtenerSucursalesActivas()
        {
            List<Sucursal> sucursales = _sucursalLogica.ObtenerActivas();
            string datos = UnirRegistros(sucursales.Select(FormatearSucursal));

            return string.Join("|",
                "OK",
                "SUCURSALES_ACTIVAS",
                datos);
        }

        private string ProcesarObtenerVehiculosPorSucursal(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud requiere el id de la sucursal.");

            int idSucursal = ObtenerEnteroRequerido(partes[1], "El id de la sucursal no es válido.");

            List<VehiculoxSucursal> inventario = _vehiculoxSucursalLogica.ObtenerPorSucursal(idSucursal);
            string datos = UnirRegistros(inventario.Select(FormatearVehiculoxSucursal));

            return string.Join("|",
                "OK",
                "VEHICULOS_POR_SUCURSAL",
                datos);
        }

        private string ProcesarRegistrarVenta(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 4, "La solicitud VENTA requiere idCliente, idSucursal e idVehiculo.");

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
                "VENTA",
                idVentaGenerado.ToString(CultureInfo.InvariantCulture));
        }

        private string ProcesarObtenerVentasPorCliente(string[] partes)
        {
            ValidarCantidadMinimaPartes(partes, 2, "La solicitud requiere el id del cliente.");

            int idCliente = ObtenerEnteroRequerido(partes[1], "El id del cliente no es válido.");

            List<Venta> ventas = _ventaLogica.ObtenerPorCliente(idCliente);
            string datos = UnirRegistros(ventas.Select(FormatearVenta));

            return string.Join("|",
                "OK",
                "VENTAS_POR_CLIENTE",
                datos);
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
                "CLIENTE_POR_ID",
                FormatearCliente(cliente));
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

            if (vehiculo.Categoria == null)
            {
                throw new InvalidOperationException("El vehículo indicado no tiene una categoría asociada válida.");
            }

            return string.Join("|",
                "OK",
                "VEHICULO_POR_ID",
                FormatearVehiculo(vehiculo));
        }

        private void ValidarCantidadMinimaPartes(string[] partes, int cantidadMinima, string mensajeError)
        {
            if (partes == null || partes.Length < cantidadMinima)
            {
                throw new ArgumentException(mensajeError);
            }
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

        private string UnirRegistros(IEnumerable<string> registros)
        {
            if (registros == null)
            {
                return string.Empty;
            }

            return string.Join(";", registros);
        }

        private string FormatearCliente(Cliente cliente)
        {
            return string.Join(",",
                cliente.IdCliente.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(cliente.Identificacion),
                EscaparCampo(cliente.NombreCompleto),
                cliente.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                cliente.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                cliente.Activo ? "1" : "0");
        }

        private string FormatearSucursal(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException(nameof(sucursal), "La sucursal indicada no es válida.");
            }

            return string.Join(",",
                sucursal.IdSucursal.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(sucursal.Nombre),
                EscaparCampo(sucursal.Direccion),
                EscaparCampo(sucursal.Telefono),
                sucursal.VendedorEncargado != null ? sucursal.VendedorEncargado.IdVendedor.ToString(CultureInfo.InvariantCulture) : "0",
                sucursal.VendedorEncargado != null ? EscaparCampo(sucursal.VendedorEncargado.NombreCompleto) : string.Empty,
                sucursal.VendedorEncargado != null ? EscaparCampo(sucursal.VendedorEncargado.Identificacion) : string.Empty,
                sucursal.Activo ? "1" : "0");
        }

        private string FormatearVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null)
            {
                throw new ArgumentNullException(nameof(vehiculo), "El vehículo indicado no es válido.");
            }

            if (vehiculo.Categoria == null)
            {
                throw new InvalidOperationException("El vehículo indicado no tiene una categoría asociada válida.");
            }

            return string.Join(",",
                vehiculo.IdVehiculo.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Marca),
                EscaparCampo(vehiculo.Modelo),
                vehiculo.Ano.ToString(CultureInfo.InvariantCulture),
                vehiculo.Precio.ToString(CultureInfo.InvariantCulture),
                vehiculo.Estado.ToString(),
                EscaparCampo(vehiculo.EstadoDescripcion),
                vehiculo.Categoria.IdCategoria.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Categoria.NombreCategoria),
                EscaparCampo(vehiculo.Categoria.Descripcion));
        }

        private string FormatearVehiculoxSucursal(VehiculoxSucursal vehiculoxSucursal)
        {
            if (vehiculoxSucursal == null)
            {
                throw new ArgumentNullException(nameof(vehiculoxSucursal), "El inventario indicado no es válido.");
            }

            if (vehiculoxSucursal.Sucursal == null)
            {
                throw new InvalidOperationException("El inventario consultado contiene una sucursal no válida.");
            }

            if (vehiculoxSucursal.Vehiculo == null)
            {
                throw new InvalidOperationException("El inventario consultado contiene un vehículo no válido.");
            }

            Vehiculo vehiculo = vehiculoxSucursal.Vehiculo;

            if (vehiculo.Categoria == null)
            {
                throw new InvalidOperationException("El inventario consultado contiene un vehículo sin categoría válida.");
            }

            return string.Join(",",
                vehiculoxSucursal.Sucursal.IdSucursal.ToString(CultureInfo.InvariantCulture),
                vehiculo.IdVehiculo.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(vehiculo.Marca),
                EscaparCampo(vehiculo.Modelo),
                vehiculo.Ano.ToString(CultureInfo.InvariantCulture),
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
            if (venta == null)
            {
                throw new ArgumentNullException(nameof(venta), "La venta indicada no es válida.");
            }

            if (venta.Cliente == null || venta.Sucursal == null || venta.Vehiculo == null)
            {
                throw new InvalidOperationException("La venta consultada contiene datos incompletos.");
            }

            if (venta.Vehiculo.Categoria == null)
            {
                throw new InvalidOperationException("La venta consultada contiene un vehículo sin categoría válida.");
            }

            return string.Join(",",
                venta.IdVenta.ToString(CultureInfo.InvariantCulture),
                venta.Cliente.IdCliente.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Cliente.NombreCompleto),
                EscaparCampo(venta.Cliente.Identificacion),
                venta.Sucursal.IdSucursal.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Sucursal.Nombre),
                venta.Vehiculo.IdVehiculo.ToString(CultureInfo.InvariantCulture),
                EscaparCampo(venta.Vehiculo.Marca),
                EscaparCampo(venta.Vehiculo.Modelo),
                venta.Vehiculo.Ano.ToString(CultureInfo.InvariantCulture),
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

            texto = texto.Replace("|", "/");
            texto = texto.Replace(";", "/");
            texto = texto.Replace(",", "/");
            texto = texto.Replace("\r", " ");
            texto = texto.Replace("\n", " ");

            return texto;
        }
    }
}