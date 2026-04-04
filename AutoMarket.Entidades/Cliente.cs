/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la entidad cliente del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-03
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class Cliente : Persona
    {
        private int _idCliente;
        private DateTime _fechaRegistro;
        private bool _activo;

        public int IdCliente
        {
            get => _idCliente;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El id del cliente debe ser mayor que cero.");
                }

                _idCliente = value;
            }
        }

        public DateTime FechaRegistro
        {
            get => _fechaRegistro;
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException("La fecha de registro del cliente es obligatoria.");
                }

                if (value.Date < FechaNacimiento.Date)
                {
                    throw new ArgumentException("La fecha de registro del cliente no puede ser anterior a la fecha de nacimiento.");
                }

                if (value.Date > DateTime.Today)
                {
                    throw new ArgumentException("La fecha de registro del cliente no puede ser futura.");
                }

                _fechaRegistro = value.Date;
            }
        }

        public bool Activo
        {
            get => _activo;
            set => _activo = value;
        }

        public Cliente(
            int idCliente,
            string identificacion,
            string nombreCompleto,
            DateTime fechaNacimiento,
            DateTime fechaRegistro,
            bool activo)
            : base(identificacion, nombreCompleto, fechaNacimiento)
        {
            IdCliente = idCliente;
            FechaRegistro = fechaRegistro;
            Activo = activo;
        }

        public override string ToString()
        {
            return IdCliente + " - " + NombreCompleto + " - " + Identificacion;
        }
    }
}