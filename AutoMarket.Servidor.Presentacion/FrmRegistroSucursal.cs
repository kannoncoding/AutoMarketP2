/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para registrar sucursales directamente en la capa lógica del servidor y persistirlas en SQL Server.
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
    public partial class FrmRegistroSucursal : Form
    {
        private readonly SucursalLogica _sucursalLogica;
        private readonly VendedorLogica _vendedorLogica;

        public FrmRegistroSucursal()
            : this(new SucursalLogica(), new VendedorLogica())
        {
        }

        public FrmRegistroSucursal(SucursalLogica sucursalLogica, VendedorLogica vendedorLogica)
        {
            _sucursalLogica = sucursalLogica ?? throw new ArgumentNullException(nameof(sucursalLogica));
            _vendedorLogica = vendedorLogica ?? throw new ArgumentNullException(nameof(vendedorLogica));

            InitializeComponent();
            ConfigurarFormulario();
        }

        private void FrmRegistroSucursal_Load(object sender, EventArgs e)
        {
            try
            {
                CargarVendedores();
                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar el formulario de sucursales.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Registro de Sucursal";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void CargarVendedores()
        {
            List<Vendedor> vendedores = _vendedorLogica.ObtenerTodos();

            cmbVendedorEncargado.DataSource = null;
            cmbVendedorEncargado.DisplayMember = nameof(Vendedor.NombreCompleto);
            cmbVendedorEncargado.ValueMember = nameof(Vendedor.IdVendedor);

            if (vendedores.Count == 0)
            {
                MessageBox.Show(
                    "No hay vendedores registrados en la base de datos. Debe registrar al menos un vendedor antes de registrar una sucursal.",
                    "Vendedores requeridos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                btnGuardar.Enabled = false;
                cmbVendedorEncargado.Enabled = false;
                return;
            }

            cmbVendedorEncargado.DataSource = vendedores;
            cmbVendedorEncargado.SelectedIndex = -1;
            cmbVendedorEncargado.Enabled = true;
            btnGuardar.Enabled = true;
        }

        private void PrepararFormularioNuevoRegistro()
        {
            txtIdSucursal.Clear();
            txtNombreSucursal.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            chkActivo.Checked = true;
            errorProviderFormulario.Clear();

            if (cmbVendedorEncargado.Items.Count > 0)
            {
                cmbVendedorEncargado.SelectedIndex = -1;
            }

            txtIdSucursal.Focus();
        }

        private bool ValidarDatos()
        {
            errorProviderFormulario.Clear();

            bool esValido = true;

            string textoIdSucursal = txtIdSucursal.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoIdSucursal))
            {
                errorProviderFormulario.SetError(txtIdSucursal, "Debe ingresar el id de la sucursal.");
                esValido = false;
            }
            else if (!int.TryParse(textoIdSucursal, out int idSucursal) || idSucursal <= 0)
            {
                errorProviderFormulario.SetError(txtIdSucursal, "Debe ingresar un id numérico mayor que cero.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreSucursal.Text))
            {
                errorProviderFormulario.SetError(txtNombreSucursal, "Debe ingresar el nombre de la sucursal.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                errorProviderFormulario.SetError(txtDireccion, "Debe ingresar la dirección de la sucursal.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                errorProviderFormulario.SetError(txtTelefono, "Debe ingresar el teléfono de la sucursal.");
                esValido = false;
            }

            if (cmbVendedorEncargado.SelectedItem == null)
            {
                errorProviderFormulario.SetError(cmbVendedorEncargado, "Debe seleccionar el vendedor encargado.");
                esValido = false;
            }

            return esValido;
        }

        private Sucursal ConstruirEntidadDesdeFormulario()
        {
            int idSucursal = Convert.ToInt32(txtIdSucursal.Text.Trim());
            string nombre = txtNombreSucursal.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            Vendedor? vendedorEncargado = cmbVendedorEncargado.SelectedItem as Vendedor;
            if (vendedorEncargado == null)
            {
                throw new InvalidOperationException("Debe seleccionar un vendedor válido.");
            }
            bool activo = chkActivo.Checked;

            return new Sucursal(
                idSucursal,
                nombre,
                direccion,
                telefono,
                vendedorEncargado,
                activo);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarDatos())
                {
                    MessageBox.Show(
                        "Debe completar correctamente los datos obligatorios antes de guardar.",
                        "Validación de datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Sucursal sucursal = ConstruirEntidadDesdeFormulario();
                _sucursalLogica.Registrar(sucursal);

                MessageBox.Show(
                    "La sucursal fue registrada correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarVendedores();
                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar la sucursal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al registrar la sucursal.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de registro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea limpiar los datos ingresados en el formulario?",
                    "Limpiar formulario",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    PrepararFormularioNuevoRegistro();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al limpiar el formulario.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error al limpiar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar el formulario de registro de sucursal?",
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

        private void txtIdSucursal_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool esControl = char.IsControl(e.KeyChar);
            bool esDigito = char.IsDigit(e.KeyChar);

            if (!esControl && !esDigito)
            {
                e.Handled = true;
            }
        }
    }
}