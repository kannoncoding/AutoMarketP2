/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de la entidad Vendedor.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class VendedorLogica
    {
        private readonly VendedorDatos _vendedorDatos;

        public VendedorLogica()
        {
            _vendedorDatos = new VendedorDatos();
        }

        public VendedorLogica(VendedorDatos vendedorDatos)
        {
            _vendedorDatos = vendedorDatos ?? throw new ArgumentNullException(nameof(vendedorDatos));
        }

        public void Registrar(Vendedor vendedor)
        {
            if (vendedor == null)
            {
                throw new ArgumentNullException(nameof(vendedor), "El vendedor es obligatorio.");
            }

            ValidarVendedor(vendedor);

            _vendedorDatos.Insertar(vendedor);
        }

        public void Actualizar(Vendedor vendedor)
        {
            if (vendedor == null)
            {
                throw new ArgumentNullException(nameof(vendedor), "El vendedor es obligatorio.");
            }

            ValidarVendedor(vendedor);

            _vendedorDatos.Actualizar(vendedor);
        }

        public void Eliminar(int idVendedor)
        {
            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
            }

            _vendedorDatos.Eliminar(idVendedor);
        }

        public Vendedor? ObtenerPorId(int idVendedor)
        {
            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
            }

            return _vendedorDatos.ObtenerPorId(idVendedor);
        }

        public Vendedor? ObtenerPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor es obligatoria.");
            }

            return _vendedorDatos.ObtenerPorIdentificacion(identificacionNormalizada);
        }

        public List<Vendedor> ObtenerTodos()
        {
            return _vendedorDatos.ObtenerTodos();
        }

        public bool ExisteId(int idVendedor)
        {
            if (idVendedor <= 0)
            {
                return false;
            }

            return _vendedorDatos.ExisteId(idVendedor);
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                return false;
            }

            return _vendedorDatos.ExisteIdentificacion(identificacionNormalizada);
        }

        private void ValidarVendedor(Vendedor vendedor)
        {
            if (vendedor.IdVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
            }

            string identificacionNormalizada = vendedor.Identificacion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor es obligatoria.");
            }

            string nombreCompletoNormalizado = vendedor.NombreCompleto?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreCompletoNormalizado))
            {
                throw new ArgumentException("El nombre completo del vendedor es obligatorio.");
            }

            if (vendedor.FechaNacimiento == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de nacimiento del vendedor es obligatoria.");
            }

            if (vendedor.FechaNacimiento.Date > DateTime.Today)
            {
                throw new ArgumentException("La fecha de nacimiento del vendedor no puede ser futura.");
            }

            if (vendedor.FechaIngreso == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de ingreso del vendedor es obligatoria.");
            }

            if (vendedor.FechaIngreso.Date < vendedor.FechaNacimiento.Date)
            {
                throw new ArgumentException("La fecha de ingreso del vendedor no puede ser anterior a la fecha de nacimiento.");
            }

            if (vendedor.FechaIngreso.Date > DateTime.Today)
            {
                throw new ArgumentException("La fecha de ingreso del vendedor no puede ser futura.");
            }

            string telefonoNormalizado = vendedor.Telefono?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(telefonoNormalizado))
            {
                throw new ArgumentException("El teléfono del vendedor es obligatorio.");
            }
        }
    }
}