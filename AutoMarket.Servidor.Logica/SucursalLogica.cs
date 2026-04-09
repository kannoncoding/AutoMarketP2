/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de la entidad Sucursal.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class SucursalLogica
    {
        private readonly SucursalDatos _sucursalDatos;

        public SucursalLogica()
        {
            _sucursalDatos = new SucursalDatos();
        }

        public SucursalLogica(SucursalDatos sucursalDatos)
        {
            _sucursalDatos = sucursalDatos ?? throw new ArgumentNullException(nameof(sucursalDatos));
        }

        public void Registrar(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException(nameof(sucursal), "La sucursal es obligatoria.");
            }

            ValidarSucursal(sucursal);

            _sucursalDatos.Insertar(sucursal);
        }

        public void Actualizar(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException(nameof(sucursal), "La sucursal es obligatoria.");
            }

            ValidarSucursal(sucursal);

            _sucursalDatos.Actualizar(sucursal);
        }

        public void Eliminar(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            _sucursalDatos.Eliminar(idSucursal);
        }

        public Sucursal? ObtenerPorId(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            return _sucursalDatos.ObtenerPorId(idSucursal);
        }

        public List<Sucursal> ObtenerTodos()
        {
            return _sucursalDatos.ObtenerTodos();
        }

        public List<Sucursal> ObtenerActivas()
        {
            return _sucursalDatos.ObtenerActivas();
        }

        public bool ExisteId(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                return false;
            }

            return _sucursalDatos.ExisteId(idSucursal);
        }

        private void ValidarSucursal(Sucursal sucursal)
        {
            if (sucursal.IdSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            string nombreNormalizado = sucursal.Nombre?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                throw new ArgumentException("El nombre de la sucursal es obligatorio.");
            }

            string direccionNormalizada = sucursal.Direccion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(direccionNormalizada))
            {
                throw new ArgumentException("La dirección de la sucursal es obligatoria.");
            }

            string telefonoNormalizado = sucursal.Telefono?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(telefonoNormalizado))
            {
                throw new ArgumentException("El teléfono de la sucursal es obligatorio.");
            }

            if (sucursal.VendedorEncargado == null)
            {
                throw new ArgumentNullException(nameof(sucursal.VendedorEncargado), "El vendedor encargado de la sucursal es obligatorio.");
            }

            if (sucursal.VendedorEncargado.IdVendedor <= 0)
            {
                throw new ArgumentException("El vendedor encargado asociado a la sucursal no es válido.");
            }

            string identificacionVendedorNormalizada = sucursal.VendedorEncargado.Identificacion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionVendedorNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor encargado es obligatoria.");
            }

            string nombreVendedorNormalizado = sucursal.VendedorEncargado.NombreCompleto?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreVendedorNormalizado))
            {
                throw new ArgumentException("El nombre completo del vendedor encargado es obligatorio.");
            }
        }
    }
}