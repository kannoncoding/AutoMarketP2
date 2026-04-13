/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar las categorías de vehículo registradas en el sistema AutoMarket directamente desde SQL Server mediante la capa lógica.
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
    public partial class FrmConsultaCategoriaVehiculo : Form
    {
        private readonly CategoriaVehiculoLogica _categoriaVehiculoLogica;

        public FrmConsultaCategoriaVehiculo()
            : this(new CategoriaVehiculoLogica())
        {
        }

        public FrmConsultaCategoriaVehiculo(CategoriaVehiculoLogica categoriaVehiculoLogica)
        {
            _categoriaVehiculoLogica = categoriaVehiculoLogica ?? throw new ArgumentNullException(nameof(categoriaVehiculoLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaCategoriaVehiculo_Load(object sender, EventArgs e)
        {
            try
            {
                CargarCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de categorías de vehículo.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Categoría de Vehículo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvCategorias.AutoGenerateColumns = false;
            dgvCategorias.ReadOnly = true;
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.AllowUserToDeleteRows = false;
            dgvCategorias.AllowUserToResizeRows = false;
            dgvCategorias.MultiSelect = false;
            dgvCategorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.RowHeadersVisible = false;
            dgvCategorias.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvCategorias.BorderStyle = BorderStyle.Fixed3D;
            dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategorias.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvCategorias.EnableHeadersVisualStyles = false;
            dgvCategorias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvCategorias.ColumnHeadersHeight = 42;
            dgvCategorias.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCategorias.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvCategorias.Columns.Clear();

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdCategoria",
                HeaderText = "Id Categoría",
                DataPropertyName = "IdCategoria",
                FillWeight = 60
            });

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombreCategoria",
                HeaderText = "Nombre Categoría",
                DataPropertyName = "NombreCategoria",
                FillWeight = 120
            });

            dgvCategorias.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDescripcion",
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion",
                FillWeight = 220
            });
        }

        private void CargarCategorias()
        {
            List<CategoriaVehiculo> categorias = _categoriaVehiculoLogica.ObtenerTodos();
            List<CategoriaVehiculoConsultaItem> items = new List<CategoriaVehiculoConsultaItem>();

            foreach (CategoriaVehiculo categoria in categorias)
            {
                items.Add(new CategoriaVehiculoConsultaItem
                {
                    IdCategoria = categoria.IdCategoria,
                    NombreCategoria = categoria.NombreCategoria,
                    Descripcion = categoria.Descripcion
                });
            }

            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarCategorias();

                MessageBox.Show(
                    "La consulta de categorías de vehículo fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de categorías de vehículo.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de consulta de categorías de vehículo?",
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

        private sealed class CategoriaVehiculoConsultaItem
        {
            public int IdCategoria { get; set; }
            public string NombreCategoria { get; set; } = string.Empty;
            public string Descripcion { get; set; } = string.Empty;
        }
    }
}