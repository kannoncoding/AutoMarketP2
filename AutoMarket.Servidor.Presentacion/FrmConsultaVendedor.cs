/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar los vendedores registrados en el sistema AutoMarket directamente desde SQL Server mediante la capa lógica.
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
    public partial class FrmConsultaVendedor : Form
    {
        private readonly VendedorLogica _vendedorLogica;

        public FrmConsultaVendedor()
            : this(new VendedorLogica())
        {
        }

        public FrmConsultaVendedor(VendedorLogica vendedorLogica)
        {
            _vendedorLogica = vendedorLogica ?? throw new ArgumentNullException(nameof(vendedorLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaVendedor_Load(object sender, EventArgs e)
        {
            try
            {
                CargarVendedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de vendedores.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Vendedor";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvVendedores.AutoGenerateColumns = false;
            dgvVendedores.ReadOnly = true;
            dgvVendedores.AllowUserToAddRows = false;
            dgvVendedores.AllowUserToDeleteRows = false;
            dgvVendedores.AllowUserToResizeRows = false;
            dgvVendedores.MultiSelect = false;
            dgvVendedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVendedores.RowHeadersVisible = false;
            dgvVendedores.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvVendedores.BorderStyle = BorderStyle.Fixed3D;
            dgvVendedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVendedores.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvVendedores.EnableHeadersVisualStyles = false;
            dgvVendedores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvVendedores.ColumnHeadersHeight = 42;
            dgvVendedores.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVendedores.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvVendedores.Columns.Clear();

            dgvVendedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdVendedor",
                HeaderText = "Id Vendedor",
                DataPropertyName = "IdVendedor",
                FillWeight = 60
            });

            dgvVendedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdentificacion",
                HeaderText = "Identificación",
                DataPropertyName = "Identificacion",
                FillWeight = 95
            });

            dgvVendedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombreCompleto",
                HeaderText = "Nombre Completo",
                DataPropertyName = "NombreCompleto",
                FillWeight = 140
            });

            dgvVendedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFechaNacimiento",
                HeaderText = "Fecha\r\nNacimiento",
                DataPropertyName = "FechaNacimientoFormato",
                FillWeight = 80
            });

            dgvVendedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFechaIngreso",
                HeaderText = "Fecha\r\nIngreso",
                DataPropertyName = "FechaIngresoFormato",
                FillWeight = 80
            });

            dgvVendedores.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                FillWeight = 85
            });
        }

        private void CargarVendedores()
        {
            List<Vendedor> vendedores = _vendedorLogica.ObtenerTodos();
            List<VendedorConsultaItem> items = new List<VendedorConsultaItem>();

            foreach (Vendedor vendedor in vendedores)
            {
                items.Add(new VendedorConsultaItem
                {
                    IdVendedor = vendedor.IdVendedor,
                    Identificacion = vendedor.Identificacion,
                    NombreCompleto = vendedor.NombreCompleto,
                    FechaNacimientoFormato = vendedor.FechaNacimiento.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FechaIngresoFormato = vendedor.FechaIngreso.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Telefono = vendedor.Telefono
                });
            }

            dgvVendedores.DataSource = null;
            dgvVendedores.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarVendedores();

                MessageBox.Show(
                    "La consulta de vendedores fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de vendedores.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de consulta de vendedores?",
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

        private sealed class VendedorConsultaItem
        {
            public int IdVendedor { get; set; }
            public string Identificacion { get; set; } = string.Empty;
            public string NombreCompleto { get; set; } = string.Empty;
            public string FechaNacimientoFormato { get; set; } = string.Empty;
            public string FechaIngresoFormato { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
        }
    }
}