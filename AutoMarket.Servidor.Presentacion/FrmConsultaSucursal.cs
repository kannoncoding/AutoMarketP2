/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar las sucursales registradas en el sistema AutoMarket directamente desde SQL Server mediante la capa lógica.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Logica;

namespace AutoMarket.Servidor.Presentacion
{
    public partial class FrmConsultaSucursal : Form
    {
        private readonly SucursalLogica _sucursalLogica;

        public FrmConsultaSucursal()
            : this(new SucursalLogica())
        {
        }

        public FrmConsultaSucursal(SucursalLogica sucursalLogica)
        {
            _sucursalLogica = sucursalLogica ?? throw new ArgumentNullException(nameof(sucursalLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaSucursal_Load(object sender, EventArgs e)
        {
            try
            {
                CargarSucursales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de sucursales.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Sucursal";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvSucursales.AutoGenerateColumns = false;
            dgvSucursales.ReadOnly = true;
            dgvSucursales.AllowUserToAddRows = false;
            dgvSucursales.AllowUserToDeleteRows = false;
            dgvSucursales.AllowUserToResizeRows = false;
            dgvSucursales.MultiSelect = false;
            dgvSucursales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSucursales.RowHeadersVisible = false;
            dgvSucursales.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvSucursales.BorderStyle = BorderStyle.Fixed3D;
            dgvSucursales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSucursales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvSucursales.EnableHeadersVisualStyles = false;

            dgvSucursales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvSucursales.ColumnHeadersHeight = 45;
            dgvSucursales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSucursales.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvSucursales.Columns.Clear();

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdSucursal",
                HeaderText = "Id Sucursal",
                DataPropertyName = "IdSucursal",
                FillWeight = 70
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                FillWeight = 120
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDireccion",
                HeaderText = "Dirección",
                DataPropertyName = "Direccion",
                FillWeight = 180
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                FillWeight = 90
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colVendedorNombre",
                HeaderText = "Vendedor (Nombre)",
                DataPropertyName = "NombreVendedorEncargado",
                FillWeight = 140
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colVendedorIdentificacion",
                HeaderText = "Vendedor\r\n(Identificación)",
                DataPropertyName = "IdentificacionVendedorEncargado",
                FillWeight = 130
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colActivo",
                HeaderText = "Activo",
                DataPropertyName = "ActivoDescripcion",
                FillWeight = 70
            });
        }

        private void CargarSucursales()
        {
            List<Sucursal> sucursales = _sucursalLogica.ObtenerTodos();
            List<SucursalConsultaItem> items = new List<SucursalConsultaItem>();

            foreach (Sucursal sucursal in sucursales)
            {
                items.Add(new SucursalConsultaItem
                {
                    IdSucursal = sucursal.IdSucursal,
                    Nombre = sucursal.Nombre,
                    Direccion = sucursal.Direccion,
                    Telefono = sucursal.Telefono,
                    NombreVendedorEncargado = sucursal.VendedorEncargado.NombreCompleto,
                    IdentificacionVendedorEncargado = sucursal.VendedorEncargado.Identificacion,
                    ActivoDescripcion = sucursal.Activo ? "Sí" : "No"
                });
            }

            dgvSucursales.DataSource = null;
            dgvSucursales.DataSource = items;

            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarSucursales();

                MessageBox.Show(
                    "La consulta de sucursales fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de sucursales.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de consulta de sucursales?",
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

        private sealed class SucursalConsultaItem
        {
            public int IdSucursal { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
            public string NombreVendedorEncargado { get; set; } = string.Empty;
            public string IdentificacionVendedorEncargado { get; set; } = string.Empty;
            public string ActivoDescripcion { get; set; } = string.Empty;
        }
    }
}