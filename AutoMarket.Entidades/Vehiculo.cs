/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la entidad vehículo del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-03
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class Vehiculo
    {
        private int _idVehiculo;
        private string _marca = string.Empty;
        private string _modelo = string.Empty;
        private int _ano;
        private decimal _precio;
        private CategoriaVehiculo _categoria = null!;
        private char _estado;

        public int IdVehiculo
        {
            get => _idVehiculo;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El id del vehículo debe ser mayor que cero.");
                }

                _idVehiculo = value;
            }
        }

        public string Marca
        {
            get => _marca;
            set
            {
                string marcaNormalizada = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(marcaNormalizada))
                {
                    throw new ArgumentException("La marca del vehículo es obligatoria.");
                }

                _marca = marcaNormalizada;
            }
        }

        public string Modelo
        {
            get => _modelo;
            set
            {
                string modeloNormalizado = value?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(modeloNormalizado))
                {
                    throw new ArgumentException("El modelo del vehículo es obligatorio.");
                }

                _modelo = modeloNormalizado;
            }
        }

        public int Ano
        {
            get => _ano;
            set
            {
                if (value < 1900)
                {
                    throw new ArgumentException("El año del vehículo no puede ser menor a 1900.");
                }

                if (value > DateTime.Today.Year + 1)
                {
                    throw new ArgumentException("El año del vehículo no es válido.");
                }

                _ano = value;
            }
        }

        public decimal Precio
        {
            get => _precio;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El precio del vehículo debe ser mayor que cero.");
                }

                _precio = value;
            }
        }

        public CategoriaVehiculo Categoria
        {
            get => _categoria;
            set => _categoria = value ?? throw new ArgumentNullException(nameof(value), "La categoría del vehículo es obligatoria.");
        }

        public char Estado
        {
            get => _estado;
            set
            {
                char estadoNormalizado = char.ToUpper(value);

                if (estadoNormalizado != 'N' && estadoNormalizado != 'U')
                {
                    throw new ArgumentException("El estado del vehículo debe ser 'N' para nuevo o 'U' para usado.");
                }

                _estado = estadoNormalizado;
            }
        }

        public string EstadoDescripcion
        {
            get
            {
                return Estado == 'N' ? "Nuevo" : "Usado";
            }
        }

        public Vehiculo(
            int idVehiculo,
            string marca,
            string modelo,
            int ano,
            decimal precio,
            CategoriaVehiculo categoria,
            char estado)
        {
            IdVehiculo = idVehiculo;
            Marca = marca;
            Modelo = modelo;
            Ano = ano;
            Precio = precio;
            Categoria = categoria;
            Estado = estado;
        }

        public override string ToString()
        {
            return IdVehiculo + " - " + Marca + " - " + Modelo;
        }
    }
}