/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de la entidad Vehiculo.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class VehiculoLogica
    {
        private readonly VehiculoDatos _vehiculoDatos;

        public VehiculoLogica()
        {
            _vehiculoDatos = new VehiculoDatos();
        }

        public VehiculoLogica(VehiculoDatos vehiculoDatos)
        {
            _vehiculoDatos = vehiculoDatos ?? throw new ArgumentNullException(nameof(vehiculoDatos));
        }

        public void Registrar(Vehiculo vehiculo)
        {
            if (vehiculo == null)
            {
                throw new ArgumentNullException(nameof(vehiculo), "El vehículo es obligatorio.");
            }

            ValidarVehiculo(vehiculo);

            _vehiculoDatos.Insertar(vehiculo);
        }

        public void Actualizar(Vehiculo vehiculo)
        {
            if (vehiculo == null)
            {
                throw new ArgumentNullException(nameof(vehiculo), "El vehículo es obligatorio.");
            }

            ValidarVehiculo(vehiculo);

            _vehiculoDatos.Actualizar(vehiculo);
        }

        public void Eliminar(int idVehiculo)
        {
            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            _vehiculoDatos.Eliminar(idVehiculo);
        }

        public Vehiculo? ObtenerPorId(int idVehiculo)
        {
            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            return _vehiculoDatos.ObtenerPorId(idVehiculo);
        }

        public List<Vehiculo> ObtenerTodos()
        {
            return _vehiculoDatos.ObtenerTodos();
        }

        public bool ExisteId(int idVehiculo)
        {
            if (idVehiculo <= 0)
            {
                return false;
            }

            return _vehiculoDatos.ExisteId(idVehiculo);
        }

        private void ValidarVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo.IdVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
            }

            string marcaNormalizada = vehiculo.Marca?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(marcaNormalizada))
            {
                throw new ArgumentException("La marca del vehículo es obligatoria.");
            }

            string modeloNormalizado = vehiculo.Modelo?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(modeloNormalizado))
            {
                throw new ArgumentException("El modelo del vehículo es obligatorio.");
            }

            if (vehiculo.Anio < 1900)
            {
                throw new ArgumentException("El año del vehículo no puede ser menor a 1900.");
            }

            if (vehiculo.Anio > DateTime.Today.Year + 1)
            {
                throw new ArgumentException("El año del vehículo no es válido.");
            }

            if (vehiculo.Precio <= 0)
            {
                throw new ArgumentException("El precio del vehículo debe ser mayor que cero.");
            }

            if (vehiculo.Categoria == null)
            {
                throw new ArgumentNullException(nameof(vehiculo.Categoria), "La categoría del vehículo es obligatoria.");
            }

            if (vehiculo.Categoria.IdCategoria <= 0)
            {
                throw new ArgumentException("La categoría asociada del vehículo no es válida.");
            }

            char estadoNormalizado = char.ToUpper(vehiculo.Estado);
            if (estadoNormalizado != 'N' && estadoNormalizado != 'U')
            {
                throw new ArgumentException("El estado del vehículo debe ser 'N' para nuevo o 'U' para usado.");
            }
        }
    }
}