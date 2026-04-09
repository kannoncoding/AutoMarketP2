/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de inventario de vehículos por sucursal.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class VehiculoxSucursalLogica
    {
        private readonly VehiculoxSucursalDatos _vehiculoxSucursalDatos;

        public VehiculoxSucursalLogica()
        {
            _vehiculoxSucursalDatos = new VehiculoxSucursalDatos();
        }

        public VehiculoxSucursalLogica(VehiculoxSucursalDatos vehiculoxSucursalDatos)
        {
            _vehiculoxSucursalDatos = vehiculoxSucursalDatos ?? throw new ArgumentNullException(nameof(vehiculoxSucursalDatos));
        }

        public void Registrar(VehiculoxSucursal vehiculoxSucursal)
        {
            if (vehiculoxSucursal == null)
            {
                throw new ArgumentNullException(nameof(vehiculoxSucursal), "La relación de vehículo por sucursal es obligatoria.");
            }

            ValidarVehiculoxSucursal(vehiculoxSucursal);

            if (!vehiculoxSucursal.Sucursal.Activo)
            {
                throw new InvalidOperationException("No se pueden asociar vehículos a una sucursal inactiva.");
            }

            _vehiculoxSucursalDatos.Insertar(vehiculoxSucursal);
        }

        public void ActualizarCantidad(int idSucursal, int idVehiculo, int nuevaCantidad)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            if (nuevaCantidad < 0)
            {
                throw new ArgumentException("La cantidad del inventario no puede ser menor que cero.");
            }

            _vehiculoxSucursalDatos.ActualizarCantidad(idSucursal, idVehiculo, nuevaCantidad);
        }

        public void Eliminar(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            _vehiculoxSucursalDatos.Eliminar(idSucursal, idVehiculo);
        }

        public VehiculoxSucursal? ObtenerRelacion(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            return _vehiculoxSucursalDatos.ObtenerRelacion(idSucursal, idVehiculo);
        }

        public List<VehiculoxSucursal> ObtenerPorSucursal(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            return _vehiculoxSucursalDatos.ObtenerPorSucursal(idSucursal);
        }

        public bool ExisteRelacion(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0 || idVehiculo <= 0)
            {
                return false;
            }

            return _vehiculoxSucursalDatos.ExisteRelacion(idSucursal, idVehiculo);
        }

        public bool TieneStock(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0)
            {
                return false;
            }

            if (idVehiculo <= 0)
            {
                return false;
            }

            return _vehiculoxSucursalDatos.TieneStock(idSucursal, idVehiculo);
        }

        public void DisminuirStock(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            if (!TieneStock(idSucursal, idVehiculo))
            {
                throw new InvalidOperationException("No hay stock disponible para el vehículo seleccionado en la sucursal indicada.");
            }

            _vehiculoxSucursalDatos.DisminuirStock(idSucursal, idVehiculo);
        }

        private void ValidarVehiculoxSucursal(VehiculoxSucursal vehiculoxSucursal)
        {
            if (vehiculoxSucursal.Sucursal == null)
            {
                throw new ArgumentNullException(nameof(vehiculoxSucursal.Sucursal), "La sucursal de la relación es obligatoria.");
            }

            if (vehiculoxSucursal.Sucursal.IdSucursal <= 0)
            {
                throw new ArgumentException("La sucursal asociada no es válida.");
            }

            string nombreSucursalNormalizado = vehiculoxSucursal.Sucursal.Nombre?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreSucursalNormalizado))
            {
                throw new ArgumentException("El nombre de la sucursal asociada es obligatorio.");
            }

            if (vehiculoxSucursal.Vehiculo == null)
            {
                throw new ArgumentNullException(nameof(vehiculoxSucursal.Vehiculo), "El vehículo de la relación es obligatorio.");
            }

            if (vehiculoxSucursal.Vehiculo.IdVehiculo <= 0)
            {
                throw new ArgumentException("El vehículo asociado no es válido.");
            }

            string marcaNormalizada = vehiculoxSucursal.Vehiculo.Marca?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(marcaNormalizada))
            {
                throw new ArgumentException("La marca del vehículo asociado es obligatoria.");
            }

            string modeloNormalizado = vehiculoxSucursal.Vehiculo.Modelo?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(modeloNormalizado))
            {
                throw new ArgumentException("El modelo del vehículo asociado es obligatorio.");
            }

            if (vehiculoxSucursal.Cantidad < 0)
            {
                throw new ArgumentException("La cantidad del inventario no puede ser menor que cero.");
            }
        }
    }
}