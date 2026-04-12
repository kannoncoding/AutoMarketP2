/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para registrar clientes directamente en la capa lógica del servidor y persistirlos en SQL Server.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Windows.Forms;
using AutoMarket.Entidades;
using AutoMarket.Servidor.Logica;

namespace AutoMarket.Servidor.Presentacion
{
    public partial class FrmRegistroCliente : Form
    {
        private readonly ClienteLogica _clienteLogica;

        public FrmRegistroCliente()
            : this(new ClienteLogica())
        {
        }

        public FrmRegistroCliente(ClienteLogica clienteLogica)
        {
            _clienteLogica = clienteLogica ?? throw new ArgumentNullException(nameof(clienteLogica));

            InitializeComponent();
            ConfigurarFormulario();
            PrepararFormularioNuevoRegistro();
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Registro de Cliente";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void PrepararFormularioNuevoRegistro()
        {
            txtIdCliente.Clear();
            txtIdentificacion.Clear();
            txtNombreCompleto.Clear();

            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);
            dtpFechaRegistro.Value = DateTime.Today;
            chkActivo.Checked = true;

            errorProviderFormulario.Clear();
            txtIdCliente.Focus();
        }

        private bool ValidarDatos()
        {
            errorProviderFormulario.Clear();

            bool esValido = true;

            string textoIdCliente = txtIdCliente.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoIdCliente))
            {
                errorProviderFormulario.SetError(txtIdCliente, "Debe ingresar el id del cliente.");
                esValido = false;
            }
            else if (!int.TryParse(textoIdCliente, out int idCliente) || idCliente <= 0)
            {
                errorProviderFormulario.SetError(txtIdCliente, "Debe ingresar un id numérico mayor que cero.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text))
            {
                errorProviderFormulario.SetError(txtIdentificacion, "Debe ingresar la identificación del cliente.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text))
            {
                errorProviderFormulario.SetError(txtNombreCompleto, "Debe ingresar el nombre completo del cliente.");
                esValido = false;
            }

            if (dtpFechaNacimiento.Value.Date > DateTime.Today)
            {
                errorProviderFormulario.SetError(dtpFechaNacimiento, "La fecha de nacimiento no puede ser futura.");
                esValido = false;
            }

            if (dtpFechaRegistro.Value.Date < dtpFechaNacimiento.Value.Date)
            {
                errorProviderFormulario.SetError(dtpFechaRegistro, "La fecha de registro no puede ser anterior a la fecha de nacimiento.");
                esValido = false;
            }

            if (dtpFechaRegistro.Value.Date > DateTime.Today)
            {
                errorProviderFormulario.SetError(dtpFechaRegistro, "La fecha de registro no puede ser futura.");
                esValido = false;
            }

            return esValido;
        }

        private Cliente ConstruirEntidadDesdeFormulario()
        {
            int idCliente = Convert.ToInt32(txtIdCliente.Text.Trim());
            string identificacion = txtIdentificacion.Text.Trim();
            string nombreCompleto = txtNombreCompleto.Text.Trim();
            DateTime fechaNacimiento = dtpFechaNacimiento.Value.Date;
            DateTime fechaRegistro = dtpFechaRegistro.Value.Date;
            bool activo = chkActivo.Checked;

            return new Cliente(
                idCliente,
                identificacion,
                nombreCompleto,
                fechaNacimiento,
                fechaRegistro,
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

                Cliente cliente = ConstruirEntidadDesdeFormulario();
                _clienteLogica.Registrar(cliente);

                MessageBox.Show(
                    "El cliente fue registrado correctamente.",
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
                    "No fue posible registrar el cliente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al registrar el cliente.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de registro de cliente?",
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

        private void txtIdCliente_KeyPress(object sender, KeyPressEventArgs e)
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