/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar las ventas registradas en el sistema AutoMarket directamente desde SQL Server mediante la capa lógica.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Logica;

namespace AutoMarket.Servidor.Presentacion
{
    public partial class FrmConsultaVenta : Form
    {
        private readonly VentaLogica _ventaLogica;

        public FrmConsultaVenta()
            : this(new VentaLogica())
        {
        }

        public FrmConsultaVenta(VentaLogica ventaLogica)
        {
            _ventaLogica = ventaLogica ?? throw new ArgumentNullException(nameof(ventaLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaVenta_Load(object sender, EventArgs e)
        {
            try
            {
                CargarVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de ventas.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Venta";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvVentas.AutoGenerateColumns = false;
            dgvVentas.ReadOnly = true;
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.AllowUserToDeleteRows = false;
            dgvVentas.AllowUserToResizeRows = false;
            dgvVentas.MultiSelect = false;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvVentas.BorderStyle = BorderStyle.Fixed3D;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvVentas.EnableHeadersVisualStyles = false;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvVentas.ColumnHeadersHeight = 44;
            dgvVentas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVentas.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvVentas.Columns.Clear();

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdVenta",
                HeaderText = "Id Venta",
                DataPropertyName = "IdVenta",
                FillWeight = 55
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClienteNombre",
                HeaderText = "Cliente",
                DataPropertyName = "NombreCliente",
                FillWeight = 120
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colClienteIdentificacion",
                HeaderText = "Cliente\r\nIdentificación",
                DataPropertyName = "IdentificacionCliente",
                FillWeight = 95
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSucursalNombre",
                HeaderText = "Sucursal",
                DataPropertyName = "NombreSucursal",
                FillWeight = 95
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colVehiculo",
                HeaderText = "Vehículo",
                DataPropertyName = "VehiculoDescripcion",
                FillWeight = 120
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoria",
                HeaderText = "Categoría",
                DataPropertyName = "NombreCategoria",
                FillWeight = 85
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstadoVehiculo",
                HeaderText = "Estado\r\nVehículo",
                DataPropertyName = "EstadoVehiculo",
                FillWeight = 65
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFechaVenta",
                HeaderText = "Fecha Venta",
                DataPropertyName = "FechaVentaFormato",
                FillWeight = 85
            });

            dgvVentas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMonto",
                HeaderText = "Monto",
                DataPropertyName = "MontoFormato",
                FillWeight = 75
            });
        }

        private void CargarVentas()
        {
            List<Venta> ventas = _ventaLogica.ObtenerTodos();
            List<VentaConsultaItem> items = new List<VentaConsultaItem>();

            foreach (Venta venta in ventas)
            {
                items.Add(new VentaConsultaItem
                {
                    IdVenta = venta.IdVenta,
                    NombreCliente = venta.Cliente.NombreCompleto,
                    IdentificacionCliente = venta.Cliente.Identificacion,
                    NombreSucursal = venta.Sucursal.Nombre,
                    VehiculoDescripcion = venta.Vehiculo.Marca + " " + venta.Vehiculo.Modelo,
                    NombreCategoria = venta.Vehiculo.Categoria.NombreCategoria,
                    EstadoVehiculo = venta.Vehiculo.EstadoDescripcion,
                    FechaVentaFormato = venta.FechaVenta.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    MontoFormato = venta.Monto.ToString("C", CultureInfo.GetCultureInfo("es-CR"))
                });
            }

            dgvVentas.DataSource = null;
            dgvVentas.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarVentas();

                MessageBox.Show(
                    "La consulta de ventas fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de ventas.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de actualización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar el formulario de consulta de ventas?",
                    "Cerrar formulario",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cerrar el formulario.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error al cerrar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private sealed class VentaConsultaItem
        {
            public int IdVenta { get; set; }
            public string NombreCliente { get; set; } = string.Empty;
            public string IdentificacionCliente { get; set; } = string.Empty;
            public string NombreSucursal { get; set; } = string.Empty;
            public string VehiculoDescripcion { get; set; } = string.Empty;
            public string NombreCategoria { get; set; } = string.Empty;
            public string EstadoVehiculo { get; set; } = string.Empty;
            public string FechaVentaFormato { get; set; } = string.Empty;
            public string MontoFormato { get; set; } = string.Empty;
        }
    }
}