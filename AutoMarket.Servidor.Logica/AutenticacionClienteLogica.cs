/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para validar la autenticación y habilitación de clientes activos en el sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class AutenticacionClienteLogica
    {
        private readonly ClienteDatos _clienteDatos;

        public AutenticacionClienteLogica()
        {
            _clienteDatos = new ClienteDatos();
        }

        public AutenticacionClienteLogica(ClienteDatos clienteDatos)
        {
            _clienteDatos = clienteDatos ?? throw new ArgumentNullException(nameof(clienteDatos));
        }

        public Cliente AutenticarPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            Cliente? cliente = _clienteDatos.ObtenerPorIdentificacion(identificacionNormalizada);

            if (cliente == null)
            {
                throw new InvalidOperationException("No existe un cliente registrado con la identificación indicada.");
            }

            if (!cliente.Activo)
            {
                throw new InvalidOperationException("El cliente existe, pero no se encuentra activo.");
            }

            return cliente;
        }

        public Cliente AutenticarPorId(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            Cliente? cliente = _clienteDatos.ObtenerPorId(idCliente);

            if (cliente == null)
            {
                throw new InvalidOperationException("No existe un cliente registrado con el id indicado.");
            }

            if (!cliente.Activo)
            {
                throw new InvalidOperationException("El cliente existe, pero no se encuentra activo.");
            }

            return cliente;
        }

        public Cliente? ObtenerClienteActivoPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            return _clienteDatos.ObtenerClienteActivoPorIdentificacion(identificacionNormalizada);
        }

        public Cliente? ObtenerClienteActivoPorId(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            Cliente? cliente = _clienteDatos.ObtenerPorId(idCliente);

            if (cliente == null)
            {
                return null;
            }

            return cliente.Activo ? cliente : null;
        }

        public bool PuedeAutenticarsePorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                return false;
            }

            Cliente? cliente = _clienteDatos.ObtenerClienteActivoPorIdentificacion(identificacionNormalizada);
            return cliente != null;
        }

        public bool PuedeAutenticarsePorId(int idCliente)
        {
            if (idCliente <= 0)
            {
                return false;
            }

            Cliente? cliente = _clienteDatos.ObtenerPorId(idCliente);
            return cliente != null && cliente.Activo;
        }

        public string ObtenerNombreClienteAutenticadoPorIdentificacion(string identificacion)
        {
            Cliente cliente = AutenticarPorIdentificacion(identificacion);
            return cliente.NombreCompleto;
        }

        public string ObtenerNombreClienteAutenticadoPorId(int idCliente)
        {
            Cliente cliente = AutenticarPorId(idCliente);
            return cliente.NombreCompleto;
        }

        public int ObtenerIdClienteAutenticadoPorIdentificacion(string identificacion)
        {
            Cliente cliente = AutenticarPorIdentificacion(identificacion);
            return cliente.IdCliente;
        }

        public string ObtenerIdentificacionClienteAutenticadoPorId(int idCliente)
        {
            Cliente cliente = AutenticarPorId(idCliente);
            return cliente.Identificacion;
        }

        public void ValidarAccesoClientePorIdentificacion(string identificacion)
        {
            AutenticarPorIdentificacion(identificacion);
        }

        public void ValidarAccesoClientePorId(int idCliente)
        {
            AutenticarPorId(idCliente);
        }
    }
}