/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase base abstracta que representa una persona del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-03
*/

using System;

namespace AutoMarket.Entidades
{
    public abstract class Persona
    {
        private string _identificacion = string.Empty;
        private string _nombreCompleto = string.Empty;
        private DateTime _fechaNacimiento;

        public string Identificacion
        {
            get => _identificacion;
            set
            {
                string identificacionNormalizada = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(identificacionNormalizada))
                {
                    throw new ArgumentException("La identificación de la persona es obligatoria.");
                }

                _identificacion = identificacionNormalizada;
            }
        }

        public string NombreCompleto
        {
            get => _nombreCompleto;
            set
            {
                string nombreNormalizado = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(nombreNormalizado))
                {
                    throw new ArgumentException("El nombre completo de la persona es obligatorio.");
                }

                _nombreCompleto = nombreNormalizado;
            }
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException("La fecha de nacimiento de la persona es obligatoria.");
                }

                if (value.Date > DateTime.Today)
                {
                    throw new ArgumentException("La fecha de nacimiento de la persona no puede ser futura.");
                }

                _fechaNacimiento = value.Date;
            }
        }

        protected Persona(string identificacion, string nombreCompleto, DateTime fechaNacimiento)
        {
            Identificacion = identificacion;
            NombreCompleto = nombreCompleto;
            FechaNacimiento = fechaNacimiento;
        }

        public override string ToString()
        {
            return NombreCompleto + " - " + Identificacion;
        }
    }
}