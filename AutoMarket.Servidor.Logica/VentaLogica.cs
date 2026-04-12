/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de la entidad Venta y el proceso de registro de ventas.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class VentaLogica
    {
        private readonly VentaDatos _ventaDatos;
        private readonly ClienteDatos _clienteDatos;
        private readonly SucursalDatos _sucursalDatos;
        private readonly VehiculoDatos _vehiculoDatos;
        private readonly VehiculoxSucursalDatos _vehiculoxSucursalDatos;

        public VentaLogica()
        {
            ConexionSqlServer conexionSqlServer = new ConexionSqlServer();

            _ventaDatos = new VentaDatos(conexionSqlServer);
            _clienteDatos = new ClienteDatos(conexionSqlServer);
            _sucursalDatos = new SucursalDatos(conexionSqlServer);
            _vehiculoDatos = new VehiculoDatos(conexionSqlServer);
            _vehiculoxSucursalDatos = new VehiculoxSucursalDatos(conexionSqlServer);
        }

        public VentaLogica(
            VentaDatos ventaDatos,
            ClienteDatos clienteDatos,
            SucursalDatos sucursalDatos,
            VehiculoDatos vehiculoDatos,
            VehiculoxSucursalDatos vehiculoxSucursalDatos)
        {
            _ventaDatos = ventaDatos ?? throw new ArgumentNullException(nameof(ventaDatos));
            _clienteDatos = clienteDatos ?? throw new ArgumentNullException(nameof(clienteDatos));
            _sucursalDatos = sucursalDatos ?? throw new ArgumentNullException(nameof(sucursalDatos));
            _vehiculoDatos = vehiculoDatos ?? throw new ArgumentNullException(nameof(vehiculoDatos));
            _vehiculoxSucursalDatos = vehiculoxSucursalDatos ?? throw new ArgumentNullException(nameof(vehiculoxSucursalDatos));
        }

        public int RegistrarVenta(Venta venta)
        {
            if (venta == null)
            {
                throw new ArgumentNullException(nameof(venta), "La venta es obligatoria.");
            }

            ValidarVenta(venta);

            Cliente clienteActivo = ObtenerClienteActivoParaRegistrarVenta(venta);

            Sucursal? sucursal = _sucursalDatos.ObtenerPorId(venta.Sucursal.IdSucursal);
            if (sucursal == null)
            {
                throw new InvalidOperationException("La sucursal indicada no existe.");
            }

            if (!sucursal.Activo)
            {
                throw new InvalidOperationException("La sucursal indicada no se encuentra activa.");
            }

            Vehiculo? vehiculo = _vehiculoDatos.ObtenerPorId(venta.Vehiculo.IdVehiculo);
            if (vehiculo == null)
            {
                throw new InvalidOperationException("El vehículo indicado no existe.");
            }

            Venta ventaPreparada = new Venta(
                clienteActivo,
                sucursal,
                vehiculo,
                venta.FechaVenta,
                vehiculo.Precio
            );

            int idVentaGenerado = _ventaDatos.RegistrarVenta(ventaPreparada);

            return idVentaGenerado;
        }

        public Venta? ObtenerPorId(int idVenta)
        {
            if (idVenta <= 0)
            {
                throw new ArgumentException("El id de la venta debe ser mayor que cero.");
            }

            return _ventaDatos.ObtenerPorId(idVenta);
        }

        public List<Venta> ObtenerTodos()
        {
            return _ventaDatos.ObtenerTodos();
        }

        public List<Venta> ObtenerPorCliente(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            return _ventaDatos.ObtenerPorCliente(idCliente);
        }

        public List<Venta> ObtenerPorSucursal(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            return _ventaDatos.ObtenerPorSucursal(idSucursal);
        }

        public bool ExisteId(int idVenta)
        {
            if (idVenta <= 0)
            {
                return false;
            }

            return _ventaDatos.ExisteId(idVenta);
        }

        public decimal ObtenerMontoVentaSugerido(int idVehiculo)
        {
            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            Vehiculo? vehiculo = _vehiculoDatos.ObtenerPorId(idVehiculo);
            if (vehiculo == null)
            {
                throw new InvalidOperationException("El vehículo indicado no existe.");
            }

            return vehiculo.Precio;
        }

        public bool PuedeRegistrarVenta(int idCliente, int idSucursal, int idVehiculo)
        {
            if (idCliente <= 0 || idSucursal <= 0 || idVehiculo <= 0)
            {
                return false;
            }

            Cliente? cliente = _clienteDatos.ObtenerPorId(idCliente);
            if (cliente == null || !cliente.Activo)
            {
                return false;
            }

            Sucursal? sucursal = _sucursalDatos.ObtenerPorId(idSucursal);
            if (sucursal == null || !sucursal.Activo)
            {
                return false;
            }

            Vehiculo? vehiculo = _vehiculoDatos.ObtenerPorId(idVehiculo);
            if (vehiculo == null)
            {
                return false;
            }

            return _vehiculoxSucursalDatos.TieneStock(idSucursal, idVehiculo);
        }

        private void ValidarVenta(Venta venta)
        {
            if (venta.Cliente == null)
            {
                throw new ArgumentNullException(nameof(venta.Cliente), "El cliente de la venta es obligatorio.");
            }

            if (venta.Cliente.IdCliente <= 0 && string.IsNullOrWhiteSpace(venta.Cliente.Identificacion?.Trim() ?? string.Empty))
            {
                throw new ArgumentException("La venta debe incluir un cliente válido.");
            }

            if (venta.Sucursal == null)
            {
                throw new ArgumentNullException(nameof(venta.Sucursal), "La sucursal de la venta es obligatoria.");
            }

            if (venta.Sucursal.IdSucursal <= 0)
            {
                throw new ArgumentException("La sucursal de la venta no es válida.");
            }

            if (venta.Vehiculo == null)
            {
                throw new ArgumentNullException(nameof(venta.Vehiculo), "El vehículo de la venta es obligatorio.");
            }

            if (venta.Vehiculo.IdVehiculo <= 0)
            {
                throw new ArgumentException("El vehículo de la venta no es válido.");
            }

            if (venta.FechaVenta == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de la venta es obligatoria.");
            }

            if (venta.FechaVenta > DateTime.Now)
            {
                throw new ArgumentException("La fecha de la venta no puede ser futura.");
            }
        }

        private Cliente ObtenerClienteActivoParaRegistrarVenta(Venta venta)
        {
            string identificacionNormalizada = venta.Cliente.Identificacion?.Trim() ?? string.Empty;
            bool tieneIdentificacion = !string.IsNullOrWhiteSpace(identificacionNormalizada);
            bool tieneIdCliente = venta.Cliente.IdCliente > 0;

            Cliente? clientePorId = null;
            if (tieneIdCliente)
            {
                clientePorId = _clienteDatos.ObtenerPorId(venta.Cliente.IdCliente);
                if (clientePorId == null || !clientePorId.Activo)
                {
                    throw new InvalidOperationException("El cliente no existe o no se encuentra activo.");
                }
            }

            Cliente? clientePorIdentificacion = null;
            if (tieneIdentificacion)
            {
                clientePorIdentificacion = _clienteDatos.ObtenerClienteActivoPorIdentificacion(identificacionNormalizada);
                if (clientePorIdentificacion == null)
                {
                    throw new InvalidOperationException("El cliente no existe o no se encuentra activo.");
                }
            }

            if (clientePorId != null && clientePorIdentificacion != null && clientePorId.IdCliente != clientePorIdentificacion.IdCliente)
            {
                throw new InvalidOperationException("El id del cliente y la identificación suministrados no corresponden al mismo cliente.");
            }

            return clientePorId ?? clientePorIdentificacion
                ?? throw new InvalidOperationException("La venta debe incluir un cliente válido y activo.");
        }
    }
}