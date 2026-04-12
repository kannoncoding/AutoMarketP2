/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #1
Descripción: Formulario de presentación para registrar categorías de vehículo directamente en la capa lógica del servidor y persistirlas en SQL Server.
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
    public partial class FrmRegistroCategoriaVehiculo : Form
    {
        private readonly CategoriaVehiculoLogica _categoriaVehiculoLogica;

        public FrmRegistroCategoriaVehiculo()
            : this(new CategoriaVehiculoLogica())
        {
        }

        public FrmRegistroCategoriaVehiculo(CategoriaVehiculoLogica categoriaVehiculoLogica)
        {
            _categoriaVehiculoLogica = categoriaVehiculoLogica ?? throw new ArgumentNullException(nameof(categoriaVehiculoLogica));

            InitializeComponent();
            ConfigurarFormulario();
            PrepararFormularioNuevoRegistro();
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Registro de Categoría de Vehículo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = SystemColors.Control;
        }

        private void PrepararFormularioNuevoRegistro()
        {
            txtIdCategoria.Clear();
            txtNombreCategoria.Clear();
            txtDescripcion.Clear();

            errorProviderFormulario.Clear();
            txtIdCategoria.Focus();
        }

        private bool ValidarDatos()
        {
            errorProviderFormulario.Clear();

            bool esValido = true;

            string textoIdCategoria = txtIdCategoria.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoIdCategoria))
            {
                errorProviderFormulario.SetError(txtIdCategoria, "Debe ingresar el id de la categoría.");
                esValido = false;
            }
            else if (!int.TryParse(textoIdCategoria, out int idCategoria) || idCategoria <= 0)
            {
                errorProviderFormulario.SetError(txtIdCategoria, "Debe ingresar un id numérico mayor que cero.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                errorProviderFormulario.SetError(txtNombreCategoria, "Debe ingresar el nombre de la categoría.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                errorProviderFormulario.SetError(txtDescripcion, "Debe ingresar la descripción de la categoría.");
                esValido = false;
            }

            return esValido;
        }

        private CategoriaVehiculo ConstruirEntidadDesdeFormulario()
        {
            int idCategoria = Convert.ToInt32(txtIdCategoria.Text.Trim());
            string nombreCategoria = txtNombreCategoria.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            return new CategoriaVehiculo(idCategoria, nombreCategoria, descripcion);
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

                CategoriaVehiculo categoriaVehiculo = ConstruirEntidadDesdeFormulario();

                _categoriaVehiculoLogica.Registrar(categoriaVehiculo);

                MessageBox.Show(
                    "La categoría de vehículo fue registrada correctamente.",
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
                    "No fue posible registrar la categoría",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al registrar la categoría de vehículo.\n\nDetalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de registro de categoría de vehículo?",
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

        private void txtIdCategoria_KeyPress(object sender, KeyPressEventArgs e)
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