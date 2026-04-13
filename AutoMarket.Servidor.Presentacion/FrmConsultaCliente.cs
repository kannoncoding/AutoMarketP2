/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar los clientes registrados en el sistema AutoMarket directamente desde SQL Server mediante la capa lógica.
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
    public partial class FrmConsultaCliente : Form
    {
        private readonly ClienteLogica _clienteLogica;

        public FrmConsultaCliente()
            : this(new ClienteLogica())
        {
        }

        public FrmConsultaCliente(ClienteLogica clienteLogica)
        {
            _clienteLogica = clienteLogica ?? throw new ArgumentNullException(nameof(clienteLogica));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
        }

        private void FrmConsultaCliente_Load(object sender, EventArgs e)
        {
            try
            {
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la consulta de clientes.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Cliente";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.AllowUserToResizeRows = false;
            dgvClientes.MultiSelect = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvClientes.BorderStyle = BorderStyle.Fixed3D;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvClientes.ColumnHeadersHeight = 42;
            dgvClientes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvClientes.Columns.Clear();

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdCliente",
                HeaderText = "Id Cliente",
                DataPropertyName = "IdCliente",
                FillWeight = 60
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdentificacion",
                HeaderText = "Identificación",
                DataPropertyName = "Identificacion",
                FillWeight = 95
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombreCompleto",
                HeaderText = "Nombre Completo",
                DataPropertyName = "NombreCompleto",
                FillWeight = 140
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFechaNacimiento",
                HeaderText = "Fecha\r\nNacimiento",
                DataPropertyName = "FechaNacimientoFormato",
                FillWeight = 80
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFechaRegistro",
                HeaderText = "Fecha\r\nRegistro",
                DataPropertyName = "FechaRegistroFormato",
                FillWeight = 80
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colActivo",
                HeaderText = "Activo",
                DataPropertyName = "ActivoDescripcion",
                FillWeight = 60
            });
        }

        private void CargarClientes()
        {
            List<Cliente> clientes = _clienteLogica.ObtenerTodos();
            List<ClienteConsultaItem> items = new List<ClienteConsultaItem>();

            foreach (Cliente cliente in clientes)
            {
                items.Add(new ClienteConsultaItem
                {
                    IdCliente = cliente.IdCliente,
                    Identificacion = cliente.Identificacion,
                    NombreCompleto = cliente.NombreCompleto,
                    FechaNacimientoFormato = cliente.FechaNacimiento.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FechaRegistroFormato = cliente.FechaRegistro.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ActivoDescripcion = cliente.Activo ? "Sí" : "No"
                });
            }

            dgvClientes.DataSource = null;
            dgvClientes.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarClientes();

                MessageBox.Show(
                    "La consulta de clientes fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al actualizar la consulta de clientes.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de consulta de clientes?",
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

        private sealed class ClienteConsultaItem
        {
            public int IdCliente { get; set; }
            public string Identificacion { get; set; } = string.Empty;
            public string NombreCompleto { get; set; } = string.Empty;
            public string FechaNacimientoFormato { get; set; } = string.Empty;
            public string FechaRegistroFormato { get; set; } = string.Empty;
            public string ActivoDescripcion { get; set; } = string.Empty;
        }
    }
}