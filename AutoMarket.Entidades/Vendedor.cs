/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la entidad vendedor del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-03
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class Vendedor : Persona
    {
        private int _idVendedor;
        private DateTime _fechaIngreso;
        private string _telefono = string.Empty;

        public int IdVendedor
        {
            get => _idVendedor;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
                }

                _idVendedor = value;
            }
        }

        public DateTime FechaIngreso
        {
            get => _fechaIngreso;
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException("La fecha de ingreso del vendedor es obligatoria.");
                }

                if (value.Date < FechaNacimiento.Date)
                {
                    throw new ArgumentException("La fecha de ingreso del vendedor no puede ser anterior a la fecha de nacimiento.");
                }

                if (value.Date > DateTime.Today)
                {
                    throw new ArgumentException("La fecha de ingreso del vendedor no puede ser futura.");
                }

                _fechaIngreso = value.Date;
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
                    throw new ArgumentException("El teléfono del vendedor es obligatorio.");
                }

                _telefono = telefonoNormalizado;
            }
        }

        public Vendedor(
            int idVendedor,
            string identificacion,
            string nombreCompleto,
            DateTime fechaNacimiento,
            DateTime fechaIngreso,
            string telefono)
            : base(identificacion, nombreCompleto, fechaNacimiento)
        {
            IdVendedor = idVendedor;
            FechaIngreso = fechaIngreso;
            Telefono = telefono;
        }

        public override string ToString()
        {
            return IdVendedor + " - " + NombreCompleto + " - " + Identificacion;
        }
    }
}