/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar el inventario de vehículos por sucursal directamente desde SQL Server mediante la capa lógica.
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
    public partial class FrmConsultaVehiculoXSucursal : Form
    {
        private readonly VehiculoxSucursalLogica _vehiculoxSucursalLogica;
        private readonly SucursalLogica _sucursalLogica;

        public FrmConsultaVehiculoXSucursal()
            : this(new VehiculoxSucursalLogica(), new SucursalLogica())
        {
        }

        public FrmConsultaVehiculoXSucursal(
            VehiculoxSucursalLogica vehiculoxSucursalLogica,
            SucursalLogica sucursalLogica)
        {
            _vehiculoxSucursalLogica = vehiculoxSucursalLogica ?? throw new ArgumentNullException(nameof(vehiculoxSucursalLogica));
            _sucursalLogica = sucursalLogica ?? throw new ArgumentNullException(nameof(sucursalLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaVehiculoXSucursal_Load(object sender, EventArgs e)
        {
            try
            {
                CargarSucursales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de vehículo por sucursal.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Vehículo por Sucursal";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvVehiculosXSucursal.AutoGenerateColumns = false;
            dgvVehiculosXSucursal.ReadOnly = true;
            dgvVehiculosXSucursal.AllowUserToAddRows = false;
            dgvVehiculosXSucursal.AllowUserToDeleteRows = false;
            dgvVehiculosXSucursal.AllowUserToResizeRows = false;
            dgvVehiculosXSucursal.MultiSelect = false;
            dgvVehiculosXSucursal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculosXSucursal.RowHeadersVisible = false;
            dgvVehiculosXSucursal.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvVehiculosXSucursal.BorderStyle = BorderStyle.Fixed3D;
            dgvVehiculosXSucursal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVehiculosXSucursal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvVehiculosXSucursal.EnableHeadersVisualStyles = false;
            dgvVehiculosXSucursal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvVehiculosXSucursal.ColumnHeadersHeight = 44;
            dgvVehiculosXSucursal.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVehiculosXSucursal.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvVehiculosXSucursal.Columns.Clear();

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdSucursal",
                HeaderText = "Id\r\nSucursal",
                DataPropertyName = "IdSucursal",
                FillWeight = 60
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombreSucursal",
                HeaderText = "Sucursal",
                DataPropertyName = "NombreSucursal",
                FillWeight = 120
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdVehiculo",
                HeaderText = "Id\r\nVehículo",
                DataPropertyName = "IdVehiculo",
                FillWeight = 60
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMarca",
                HeaderText = "Marca",
                DataPropertyName = "Marca",
                FillWeight = 85
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelo",
                HeaderText = "Modelo",
                DataPropertyName = "Modelo",
                FillWeight = 100
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAno",
                HeaderText = "Año",
                DataPropertyName = "Ano",
                FillWeight = 55
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrecio",
                HeaderText = "Precio",
                DataPropertyName = "PrecioFormato",
                FillWeight = 85
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                DataPropertyName = "EstadoDescripcion",
                FillWeight = 70
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoriaNombre",
                HeaderText = "Categoría\r\n(Nombre)",
                DataPropertyName = "NombreCategoria",
                FillWeight = 95
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoriaDescripcion",
                HeaderText = "Categoría\r\n(Descripción)",
                DataPropertyName = "DescripcionCategoria",
                FillWeight = 140
            });

            dgvVehiculosXSucursal.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCantidad",
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                FillWeight = 60
            });
        }

        private void CargarSucursales()
        {
            List<Sucursal> sucursales = _sucursalLogica.ObtenerTodos();

            cmbSucursal.DataSource = null;
            cmbSucursal.DisplayMember = nameof(Sucursal.Nombre);
            cmbSucursal.ValueMember = nameof(Sucursal.IdSucursal);

            if (sucursales.Count == 0)
            {
                cmbSucursal.Enabled = false;
                btnActualizar.Enabled = false;

                dgvVehiculosXSucursal.DataSource = null;
                lblCantidadRegistrosValor.Text = "0";

                MessageBox.Show(
                    "No hay sucursales registradas en la base de datos para consultar inventario.",
                    "Sin sucursales",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            cmbSucursal.DataSource = sucursales;
            cmbSucursal.SelectedIndex = 0;
            cmbSucursal.Enabled = true;
            btnActualizar.Enabled = true;

            CargarInventarioPorSucursal();
        }

        private void CargarInventarioPorSucursal()
        {
            Sucursal? sucursalSeleccionada = cmbSucursal.SelectedItem as Sucursal;
            if (sucursalSeleccionada == null)
            {
                dgvVehiculosXSucursal.DataSource = null;
                lblCantidadRegistrosValor.Text = "0";
                return;
            }

            List<VehiculoxSucursal> inventario = _vehiculoxSucursalLogica.ObtenerPorSucursal(sucursalSeleccionada.IdSucursal);
            List<VehiculoXSucursalConsultaItem> items = new List<VehiculoXSucursalConsultaItem>();

            foreach (VehiculoxSucursal item in inventario)
            {
                items.Add(new VehiculoXSucursalConsultaItem
                {
                    IdSucursal = item.Sucursal.IdSucursal,
                    NombreSucursal = item.Sucursal.Nombre,
                    IdVehiculo = item.Vehiculo.IdVehiculo,
                    Marca = item.Vehiculo.Marca,
                    Modelo = item.Vehiculo.Modelo,
                    Ano = item.Vehiculo.Ano,
                    PrecioFormato = item.Vehiculo.Precio.ToString("C", CultureInfo.GetCultureInfo("es-CR")),
                    EstadoDescripcion = item.Vehiculo.EstadoDescripcion,
                    NombreCategoria = item.Vehiculo.Categoria.NombreCategoria,
                    DescripcionCategoria = item.Vehiculo.Categoria.Descripcion,
                    Cantidad = item.Cantidad
                });
            }

            dgvVehiculosXSucursal.DataSource = null;
            dgvVehiculosXSucursal.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void cmbSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            try
            {
                CargarInventarioPorSucursal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al consultar el inventario de la sucursal seleccionada.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de consulta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarSucursales();

                MessageBox.Show(
                    "La consulta de vehículo por sucursal fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de vehículo por sucursal.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de consulta de vehículo por sucursal?",
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

        private sealed class VehiculoXSucursalConsultaItem
        {
            public int IdSucursal { get; set; }
            public string NombreSucursal { get; set; } = string.Empty;
            public int IdVehiculo { get; set; }
            public string Marca { get; set; } = string.Empty;
            public string Modelo { get; set; } = string.Empty;
            public int Ano { get; set; }
            public string PrecioFormato { get; set; } = string.Empty;
            public string EstadoDescripcion { get; set; } = string.Empty;
            public string NombreCategoria { get; set; } = string.Empty;
            public string DescripcionCategoria { get; set; } = string.Empty;
            public int Cantidad { get; set; }
        }
    }
}