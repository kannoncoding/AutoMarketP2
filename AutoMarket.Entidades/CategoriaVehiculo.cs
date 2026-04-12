/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la entidad categoría de vehículo del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-03
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class CategoriaVehiculo
    {
        private int _idCategoria;
        private string _nombreCategoria = string.Empty;
        private string _descripcion = string.Empty;

        public int IdCategoria
        {
            get => _idCategoria;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El id de la categoría debe ser mayor que cero.");
                }

                _idCategoria = value;
            }
        }

        public string NombreCategoria
        {
            get => _nombreCategoria;
            set
            {
                string nombreNormalizado = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombreNormalizado))
                {
                    throw new ArgumentException("El nombre de la categoría es obligatorio.");
                }

                _nombreCategoria = nombreNormalizado;
            }
        }

        public string Descripcion
        {
            get => _descripcion;
            set
            {
                string descripcionNormalizada = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(descripcionNormalizada))
                {
                    throw new ArgumentException("La descripción de la categoría es obligatoria.");
                }

                _descripcion = descripcionNormalizada;
            }
        }

        public CategoriaVehiculo(int idCategoria, string nombreCategoria, string descripcion)
        {
            IdCategoria = idCategoria;
            NombreCategoria = nombreCategoria;
            Descripcion = descripcion;
        }

        public CategoriaVehiculo(string nombreCategoria, string descripcion)
        {
            NombreCategoria = nombreCategoria;
            Descripcion = descripcion;
        }

        public override string ToString()
        {
            return IdCategoria + " - " + NombreCategoria;
        }
    }
}