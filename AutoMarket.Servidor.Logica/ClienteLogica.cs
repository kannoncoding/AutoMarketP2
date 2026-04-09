/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica de negocio para administrar las operaciones de la entidad Cliente.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-05
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Logica
{
    public sealed class ClienteLogica
    {
        private readonly ClienteDatos _clienteDatos;

        public ClienteLogica()
        {
            _clienteDatos = new ClienteDatos();
        }

        public ClienteLogica(ClienteDatos clienteDatos)
        {
            _clienteDatos = clienteDatos ?? throw new ArgumentNullException(nameof(clienteDatos));
        }

        public void Registrar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente es obligatorio.");
            }

            ValidarCliente(cliente);

            _clienteDatos.Insertar(cliente);
        }

        public void Actualizar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente es obligatorio.");
            }

            ValidarCliente(cliente);

            _clienteDatos.Actualizar(cliente);
        }

        public void Eliminar(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            _clienteDatos.Eliminar(idCliente);
        }

        public Cliente? ObtenerPorId(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            return _clienteDatos.ObtenerPorId(idCliente);
        }

        public Cliente? ObtenerPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            return _clienteDatos.ObtenerPorIdentificacion(identificacionNormalizada);
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

        public List<Cliente> ObtenerTodos()
        {
            return _clienteDatos.ObtenerTodos();
        }

        public bool ExisteId(int idCliente)
        {
            if (idCliente <= 0)
            {
                return false;
            }

            return _clienteDatos.ExisteId(idCliente);
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                return false;
            }

            return _clienteDatos.ExisteIdentificacion(identificacionNormalizada);
        }

        public bool ClienteEstaActivo(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                return false;
            }

            Cliente? cliente = _clienteDatos.ObtenerClienteActivoPorIdentificacion(identificacionNormalizada);
            return cliente != null;
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (cliente.IdCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            string identificacionNormalizada = cliente.Identificacion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            string nombreCompletoNormalizado = cliente.NombreCompleto?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreCompletoNormalizado))
            {
                throw new ArgumentException("El nombre completo del cliente es obligatorio.");
            }

            if (cliente.FechaNacimiento == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de nacimiento del cliente es obligatoria.");
            }

            if (cliente.FechaNacimiento.Date > DateTime.Today)
            {
                throw new ArgumentException("La fecha de nacimiento del cliente no puede ser futura.");
            }

            if (cliente.FechaRegistro == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de registro del cliente es obligatoria.");
            }

            if (cliente.FechaRegistro.Date < cliente.FechaNacimiento.Date)
            {
                throw new ArgumentException("La fecha de registro del cliente no puede ser anterior a la fecha de nacimiento.");
            }

            if (cliente.FechaRegistro.Date > DateTime.Today)
            {
                throw new ArgumentException("La fecha de registro del cliente no puede ser futura.");
            }
        }
    }
}