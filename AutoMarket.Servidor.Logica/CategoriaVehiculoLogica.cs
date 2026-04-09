/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de la entidad CategoriaVehiculo.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class CategoriaVehiculoLogica
    {
        private readonly CategoriaVehiculoDatos _categoriaVehiculoDatos;

        public CategoriaVehiculoLogica()
        {
            _categoriaVehiculoDatos = new CategoriaVehiculoDatos();
        }

        public CategoriaVehiculoLogica(CategoriaVehiculoDatos categoriaVehiculoDatos)
        {
            _categoriaVehiculoDatos = categoriaVehiculoDatos ?? throw new ArgumentNullException(nameof(categoriaVehiculoDatos));
        }

        public void Registrar(CategoriaVehiculo categoriaVehiculo)
        {
            if (categoriaVehiculo == null)
            {
                throw new ArgumentNullException(nameof(categoriaVehiculo), "La categoría de vehículo es obligatoria.");
            }

            ValidarCategoriaVehiculo(categoriaVehiculo);

            _categoriaVehiculoDatos.Insertar(categoriaVehiculo);
        }

        public void Actualizar(CategoriaVehiculo categoriaVehiculo)
        {
            if (categoriaVehiculo == null)
            {
                throw new ArgumentNullException(nameof(categoriaVehiculo), "La categoría de vehículo es obligatoria.");
            }

            ValidarCategoriaVehiculo(categoriaVehiculo);

            _categoriaVehiculoDatos.Actualizar(categoriaVehiculo);
        }

        public void Eliminar(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                throw new ArgumentException("El id de la categoría debe ser mayor que cero.");
            }

            _categoriaVehiculoDatos.Eliminar(idCategoria);
        }

        public CategoriaVehiculo? ObtenerPorId(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                throw new ArgumentException("El id de la categoría debe ser mayor que cero.");
            }

            return _categoriaVehiculoDatos.ObtenerPorId(idCategoria);
        }

        public List<CategoriaVehiculo> ObtenerTodos()
        {
            return _categoriaVehiculoDatos.ObtenerTodos();
        }

        public bool ExisteId(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                return false;
            }

            return _categoriaVehiculoDatos.ExisteId(idCategoria);
        }

        private void ValidarCategoriaVehiculo(CategoriaVehiculo categoriaVehiculo)
        {
            if (categoriaVehiculo.IdCategoria <= 0)
            {
                throw new ArgumentException("El id de la categoría debe ser mayor que cero.");
            }

            string nombreCategoriaNormalizado = categoriaVehiculo.NombreCategoria?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreCategoriaNormalizado))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio.");
            }

            string descripcionNormalizada = categoriaVehiculo.Descripcion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(descripcionNormalizada))
            {
                throw new ArgumentException("La descripción de la categoría es obligatoria.");
            }
        }
    }
}