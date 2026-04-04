/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la entidad sucursal del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-03
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class Sucursal
    {
        private int _idSucursal;
        private string _nombre = string.Empty;
        private string _direccion = string.Empty;
        private string _telefono = string.Empty;
        private Vendedor _vendedorEncargado = null!;
        private bool _activo;

        public int IdSucursal
        {
            get => _idSucursal;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
                }

                _idSucursal = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                string nombreNormalizado = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombreNormalizado))
                {
                    throw new ArgumentException("El nombre de la sucursal es obligatorio.");
                }

                _nombre = nombreNormalizado;
            }
        }

        public string Direccion
        {
            get => _direccion;
            set
            {
                string direccionNormalizada = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(direccionNormalizada))
                {
                    throw new ArgumentException("La dirección de la sucursal es obligatoria.");
                }

                _direccion = direccionNormalizada;
            }
        }

        public string Telefono
        {
            get => _telefono;
            set
            {
                string telefonoNormalizado = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(telefonoNormalizado))
                {
                    throw new ArgumentException("El teléfono de la sucursal es obligatorio.");
                }

                _telefono = telefonoNormalizado;
            }
        }

        public Vendedor VendedorEncargado
        {
            get => _vendedorEncargado;
            set => _vendedorEncargado = value ?? throw new ArgumentNullException(nameof(value), "El vendedor encargado de la sucursal es obligatorio.");
        }

        public bool Activo
        {
            get => _activo;
            set => _activo = value;
        }

        public Sucursal(
            int idSucursal,
            string nombre,
            string direccion,
            string telefono,
            Vendedor vendedorEncargado,
            bool activo)
        {
            IdSucursal = idSucursal;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            VendedorEncargado = vendedorEncargado;
            Activo = activo;
        }

        public override string ToString()
        {
            return IdSucursal + " - " + Nombre;
        }
    }
}