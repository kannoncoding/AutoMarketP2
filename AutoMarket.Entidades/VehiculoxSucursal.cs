/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la asociación entre un vehículo y una sucursal dentro del inventario del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class VehiculoxSucursal
    {
        private Sucursal _sucursal = null!;
        private Vehiculo _vehiculo = null!;
        private int _cantidad;

        public Sucursal Sucursal
        {
            get => _sucursal;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "La sucursal de la asociación es obligatoria.");
                }

                _sucursal = value;
            }
        }

        public Vehiculo Vehiculo
        {
            get => _vehiculo;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "El vehículo de la asociación es obligatorio.");
                }

                _vehiculo = value;
            }
        }

        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("La cantidad de vehículos en sucursal no puede ser menor que cero.");
                }

                _cantidad = value;
            }
        }

        public VehiculoxSucursal(Sucursal sucursal, Vehiculo vehiculo, int cantidad)
        {
            Sucursal = sucursal;
            Vehiculo = vehiculo;
            Cantidad = cantidad;
        }

        public override string ToString()
        {
            return Sucursal.Nombre + " - " + Vehiculo.Marca + " " + Vehiculo.Modelo + " - Cantidad: " + Cantidad;
        }
    }
}