/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar los vehículos registrados en el sistema AutoMarket directamente desde SQL Server mediante la capa lógica.
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
    public partial class FrmConsultaVehiculo : Form
    {
        private readonly VehiculoLogica _vehiculoLogica;

        public FrmConsultaVehiculo()
            : this(new VehiculoLogica())
        {
        }

        public FrmConsultaVehiculo(VehiculoLogica vehiculoLogica)
        {
            _vehiculoLogica = vehiculoLogica ?? throw new ArgumentNullException(nameof(vehiculoLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaVehiculo_Load(object sender, EventArgs e)
        {
            try
            {
                CargarVehiculos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de vehículos.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Vehículo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvVehiculos.AutoGenerateColumns = false;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.AllowUserToDeleteRows = false;
            dgvVehiculos.AllowUserToResizeRows = false;
            dgvVehiculos.MultiSelect = false;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.RowHeadersVisible = false;
            dgvVehiculos.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvVehiculos.BorderStyle = BorderStyle.Fixed3D;
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVehiculos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvVehiculos.EnableHeadersVisualStyles = false;
            dgvVehiculos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvVehiculos.ColumnHeadersHeight = 44;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvVehiculos.Columns.Clear();

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdVehiculo",
                HeaderText = "Id Vehículo",
                DataPropertyName = "IdVehiculo",
                FillWeight = 70
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMarca",
                HeaderText = "Marca",
                DataPropertyName = "Marca",
                FillWeight = 95
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelo",
                HeaderText = "Modelo",
                DataPropertyName = "Modelo",
                FillWeight = 110
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAno",
                HeaderText = "Año",
                DataPropertyName = "Ano",
                FillWeight = 60
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrecio",
                HeaderText = "Precio",
                DataPropertyName = "PrecioFormato",
                FillWeight = 90
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                DataPropertyName = "EstadoDescripcion",
                FillWeight = 70
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoriaNombre",
                HeaderText = "Categoría\r\n(Nombre)",
                DataPropertyName = "NombreCategoria",
                FillWeight = 115
            });

            dgvVehiculos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoriaDescripcion",
                HeaderText = "Categoría\r\n(Descripción)",
                DataPropertyName = "DescripcionCategoria",
                FillWeight = 170
            });
        }

        private void CargarVehiculos()
        {
            List<Vehiculo> vehiculos = _vehiculoLogica.ObtenerTodos();
            List<VehiculoConsultaItem> items = new List<VehiculoConsultaItem>();

            foreach (Vehiculo vehiculo in vehiculos)
            {
                items.Add(new VehiculoConsultaItem
                {
                    IdVehiculo = vehiculo.IdVehiculo,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Ano = vehiculo.Ano,
                    PrecioFormato = vehiculo.Precio.ToString("C", CultureInfo.GetCultureInfo("es-CR")),
                    EstadoDescripcion = vehiculo.EstadoDescripcion,
                    NombreCategoria = vehiculo.Categoria.NombreCategoria,
                    DescripcionCategoria = vehiculo.Categoria.Descripcion
                });
            }

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarVehiculos();

                MessageBox.Show(
                    "La consulta de vehículos fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de vehículos.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de consulta de vehículos?",
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

        private sealed class VehiculoConsultaItem
        {
            public int IdVehiculo { get; set; }
            public string Marca { get; set; } = string.Empty;
            public string Modelo { get; set; } = string.Empty;
            public int Ano { get; set; }
            public string PrecioFormato { get; set; } = string.Empty;
            public string EstadoDescripcion { get; set; } = string.Empty;
            public string NombreCategoria { get; set; } = string.Empty;
            public string DescripcionCategoria { get; set; } = string.Empty;
        }
    }
}