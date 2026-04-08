/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa la entidad venta del sistema AutoMarket.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;

namespace AutoMarket.Entidades
{
    public sealed class Venta
    {
        private int _idVenta;
        private Cliente _cliente = null!;
        private Sucursal _sucursal = null!;
        private Vehiculo _vehiculo = null!;
        private DateTime _fechaVenta;
        private decimal _monto;

        public int IdVenta
        {
            get => _idVenta;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("El id de la venta no puede ser negativo.");
                }

                _idVenta = value;
            }
        }

        public Cliente Cliente
        {
            get => _cliente;
            set => _cliente = value ?? throw new ArgumentNullException(nameof(value), "El cliente de la venta es obligatorio.");
        }

        public Sucursal Sucursal
        {
            get => _sucursal;
            set => _sucursal = value ?? throw new ArgumentNullException(nameof(value), "La sucursal de la venta es obligatoria.");
        }

        public Vehiculo Vehiculo
        {
            get => _vehiculo;
            set => _vehiculo = value ?? throw new ArgumentNullException(nameof(value), "El vehículo de la venta es obligatorio.");
        }

        public DateTime FechaVenta
        {
            get => _fechaVenta;
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException("La fecha de la venta es obligatoria.");
                }

                if (value > DateTime.Now)
                {
                    throw new ArgumentException("La fecha de la venta no puede ser futura.");
                }

                _fechaVenta = value;
            }
        }

        public decimal Monto
        {
            get => _monto;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El monto de la venta debe ser mayor que cero.");
                }

                _monto = value;
            }
        }

        public Venta(
            int idVenta,
            Cliente cliente,
            Sucursal sucursal,
            Vehiculo vehiculo,
            DateTime fechaVenta,
            decimal monto)
        {
            IdVenta = idVenta;
            Cliente = cliente;
            Sucursal = sucursal;
            Vehiculo = vehiculo;
            FechaVenta = fechaVenta;
            Monto = monto;
        }

        public Venta(
            Cliente cliente,
            Sucursal sucursal,
            Vehiculo vehiculo,
            DateTime fechaVenta,
            decimal monto)
            : this(0, cliente, sucursal, vehiculo, fechaVenta, monto)
        {
        }

        public override string ToString()
        {
            string textoIdVenta = IdVenta > 0 ? IdVenta.ToString() : "Nueva venta";
            return textoIdVenta + " - " + Cliente.NombreCompleto + " - " + Vehiculo.Marca + " " + Vehiculo.Modelo;
        }
    }
}