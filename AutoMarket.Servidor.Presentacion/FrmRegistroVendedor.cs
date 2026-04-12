/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para registrar vendedores directamente en la capa lógica del servidor y persistirlos en SQL Server.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Logica;

namespace AutoMarket.Servidor.Presentacion
{
    public partial class FrmRegistroVendedor : Form
    {
        private readonly VendedorLogica _vendedorLogica;

        public FrmRegistroVendedor()
            : this(new VendedorLogica())
        {
        }

        public FrmRegistroVendedor(VendedorLogica vendedorLogica)
        {
            _vendedorLogica = vendedorLogica ?? throw new ArgumentNullException(nameof(vendedorLogica));

            InitializeComponent();
            ConfigurarFormulario();
            PrepararFormularioNuevoRegistro();
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Registro de Vendedor";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = SystemColors.Control;
        }

        private void PrepararFormularioNuevoRegistro()
        {
            txtIdVendedor.Clear();
            txtIdentificacion.Clear();
            txtNombreCompleto.Clear();
            txtTelefono.Clear();

            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);
            dtpFechaIngreso.Value = DateTime.Today;

            errorProviderFormulario.Clear();
            txtIdVendedor.Focus();
        }

        private bool ValidarDatos()
        {
            errorProviderFormulario.Clear();

            bool esValido = true;

            string textoIdVendedor = txtIdVendedor.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoIdVendedor))
            {
                errorProviderFormulario.SetError(txtIdVendedor, "Debe ingresar el id del vendedor.");
                esValido = false;
            }
            else if (!int.TryParse(textoIdVendedor, out int idVendedor) || idVendedor <= 0)
            {
                errorProviderFormulario.SetError(txtIdVendedor, "Debe ingresar un id numérico mayor que cero.");
                esValido = false;
            }

            string identificacion = txtIdentificacion.Text.Trim();
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                errorProviderFormulario.SetError(txtIdentificacion, "Debe ingresar la identificación del vendedor.");
                esValido = false;
            }

            string nombreCompleto = txtNombreCompleto.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                errorProviderFormulario.SetError(txtNombreCompleto, "Debe ingresar el nombre completo del vendedor.");
                esValido = false;
            }

            if (dtpFechaNacimiento.Value.Date > DateTime.Today)
            {
                errorProviderFormulario.SetError(dtpFechaNacimiento, "La fecha de nacimiento no puede ser futura.");
                esValido = false;
            }

            if (dtpFechaIngreso.Value.Date < dtpFechaNacimiento.Value.Date)
            {
                errorProviderFormulario.SetError(dtpFechaIngreso, "La fecha de ingreso no puede ser anterior a la fecha de nacimiento.");
                esValido = false;
            }

            if (dtpFechaIngreso.Value.Date > DateTime.Today)
            {
                errorProviderFormulario.SetError(dtpFechaIngreso, "La fecha de ingreso no puede ser futura.");
                esValido = false;
            }

            string telefono = txtTelefono.Text.Trim();
            if (string.IsNullOrWhiteSpace(telefono))
            {
                errorProviderFormulario.SetError(txtTelefono, "Debe ingresar el teléfono del vendedor.");
                esValido = false;
            }

            return esValido;
        }

        private Vendedor ConstruirEntidadDesdeFormulario()
        {
            int idVendedor = Convert.ToInt32(txtIdVendedor.Text.Trim());
            string identificacion = txtIdentificacion.Text.Trim();
            string nombreCompleto = txtNombreCompleto.Text.Trim();
            DateTime fechaNacimiento = dtpFechaNacimiento.Value.Date;
            DateTime fechaIngreso = dtpFechaIngreso.Value.Date;
            string telefono = txtTelefono.Text.Trim();

            return new Vendedor(
                idVendedor,
                identificacion,
                nombreCompleto,
                fechaNacimiento,
                fechaIngreso,
                telefono);
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

                Vendedor vendedor = ConstruirEntidadDesdeFormulario();

                _vendedorLogica.Registrar(vendedor);

                MessageBox.Show(
                    "El vendedor fue registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar el vendedor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al registrar el vendedor.\n\nDetalle: {ex.Message}",
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
                    $"Ocurrió un error al limpiar el formulario.\n\nDetalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de registro de vendedor?",
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
                    $"Ocurrió un error al cerrar el formulario.\n\nDetalle: {ex.Message}",
                    "Error al cerrar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void txtIdVendedor_KeyPress(object sender, KeyPressEventArgs e)
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