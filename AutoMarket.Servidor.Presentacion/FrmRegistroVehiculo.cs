/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para registrar vehículos directamente en la capa lógica del servidor y persistirlos en SQL Server.
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
    public partial class FrmRegistroVehiculo : Form
    {
        private readonly VehiculoLogica _vehiculoLogica;
        private readonly CategoriaVehiculoLogica _categoriaVehiculoLogica;

        public FrmRegistroVehiculo()
            : this(new VehiculoLogica(), new CategoriaVehiculoLogica())
        {
        }

        public FrmRegistroVehiculo(VehiculoLogica vehiculoLogica, CategoriaVehiculoLogica categoriaVehiculoLogica)
        {
            _vehiculoLogica = vehiculoLogica ?? throw new ArgumentNullException(nameof(vehiculoLogica));
            _categoriaVehiculoLogica = categoriaVehiculoLogica ?? throw new ArgumentNullException(nameof(categoriaVehiculoLogica));

            InitializeComponent();
            ConfigurarFormulario();
        }

        private void FrmRegistroVehiculo_Load(object sender, EventArgs e)
        {
            try
            {
                CargarCategorias();
                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar el formulario de vehículos.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Registro de Vehículo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void CargarCategorias()
        {
            List<CategoriaVehiculo> categorias = _categoriaVehiculoLogica.ObtenerTodos();

            cmbCategoria.DataSource = null;
            cmbCategoria.DisplayMember = nameof(CategoriaVehiculo.NombreCategoria);
            cmbCategoria.ValueMember = nameof(CategoriaVehiculo.IdCategoria);

            if (categorias.Count == 0)
            {
                MessageBox.Show(
                    "No hay categorías registradas en la base de datos. Debe registrar al menos una categoría antes de registrar un vehículo.",
                    "Categorías requeridas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                btnGuardar.Enabled = false;
                cmbCategoria.Enabled = false;
                return;
            }

            cmbCategoria.DataSource = categorias;
            cmbCategoria.SelectedIndex = -1;
            cmbCategoria.Enabled = true;
            btnGuardar.Enabled = true;
        }

        private void PrepararFormularioNuevoRegistro()
        {
            txtIdVehiculo.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            txtAno.Clear();
            txtPrecio.Clear();

            if (cmbCategoria.Items.Count > 0)
            {
                cmbCategoria.SelectedIndex = -1;
            }

            rdbNuevo.Checked = true;
            errorProviderFormulario.Clear();
            txtIdVehiculo.Focus();
        }

        private bool ValidarDatos()
        {
            errorProviderFormulario.Clear();

            bool esValido = true;

            string textoIdVehiculo = txtIdVehiculo.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoIdVehiculo))
            {
                errorProviderFormulario.SetError(txtIdVehiculo, "Debe ingresar el id del vehículo.");
                esValido = false;
            }
            else if (!int.TryParse(textoIdVehiculo, out int idVehiculo) || idVehiculo <= 0)
            {
                errorProviderFormulario.SetError(txtIdVehiculo, "Debe ingresar un id numérico mayor que cero.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                errorProviderFormulario.SetError(txtMarca, "Debe ingresar la marca del vehículo.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                errorProviderFormulario.SetError(txtModelo, "Debe ingresar el modelo del vehículo.");
                esValido = false;
            }

            string textoAno = txtAno.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoAno))
            {
                errorProviderFormulario.SetError(txtAno, "Debe ingresar el año del vehículo.");
                esValido = false;
            }
            else if (!int.TryParse(textoAno, out int ano))
            {
                errorProviderFormulario.SetError(txtAno, "Debe ingresar un año válido.");
                esValido = false;
            }
            else if (ano < 1900 || ano > DateTime.Today.Year + 1)
            {
                errorProviderFormulario.SetError(txtAno, "Debe ingresar un año entre 1900 y el próximo año.");
                esValido = false;
            }

            string textoPrecio = txtPrecio.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoPrecio))
            {
                errorProviderFormulario.SetError(txtPrecio, "Debe ingresar el precio del vehículo.");
                esValido = false;
            }
            else if (!decimal.TryParse(textoPrecio, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precio) &&
                     !decimal.TryParse(textoPrecio, NumberStyles.Number, CultureInfo.InvariantCulture, out precio))
            {
                errorProviderFormulario.SetError(txtPrecio, "Debe ingresar un precio válido.");
                esValido = false;
            }
            else if (precio <= 0)
            {
                errorProviderFormulario.SetError(txtPrecio, "El precio debe ser mayor que cero.");
                esValido = false;
            }

            if (cmbCategoria.SelectedItem == null)
            {
                errorProviderFormulario.SetError(cmbCategoria, "Debe seleccionar la categoría del vehículo.");
                esValido = false;
            }

            if (!rdbNuevo.Checked && !rdbUsado.Checked)
            {
                errorProviderFormulario.SetError(grpEstado, "Debe seleccionar el estado del vehículo.");
                esValido = false;
            }

            return esValido;
        }

        private Vehiculo ConstruirEntidadDesdeFormulario()
        {
            int idVehiculo = Convert.ToInt32(txtIdVehiculo.Text.Trim());
            string marca = txtMarca.Text.Trim();
            string modelo = txtModelo.Text.Trim();
            int ano = Convert.ToInt32(txtAno.Text.Trim());

            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out precio))
            {
                precio = decimal.Parse(txtPrecio.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture);
            }

            CategoriaVehiculo? categoria = cmbCategoria.SelectedItem as CategoriaVehiculo;
            if (categoria == null)
            {
                throw new InvalidOperationException("Debe seleccionar una categoría válida.");
            }

            char estado = rdbNuevo.Checked ? 'N' : 'U';

            return new Vehiculo(
                idVehiculo,
                marca,
                modelo,
                ano,
                precio,
                categoria,
                estado);
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

                Vehiculo vehiculo = ConstruirEntidadDesdeFormulario();
                _vehiculoLogica.Registrar(vehiculo);

                MessageBox.Show(
                    "El vehículo fue registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCategorias();
                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar el vehículo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al registrar el vehículo.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de registro de vehículo?",
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

        private void txtIdVehiculo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool esControl = char.IsControl(e.KeyChar);
            bool esDigito = char.IsDigit(e.KeyChar);

            if (!esControl && !esDigito)
            {
                e.Handled = true;
            }
        }

        private void txtAno_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool esControl = char.IsControl(e.KeyChar);
            bool esDigito = char.IsDigit(e.KeyChar);

            if (!esControl && !esDigito)
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool esControl = char.IsControl(e.KeyChar);
            bool esDigito = char.IsDigit(e.KeyChar);
            bool esSeparadorDecimal = e.KeyChar == ',' || e.KeyChar == '.';

            if (!esControl && !esDigito && !esSeparadorDecimal)
            {
                e.Handled = true;
            }

            if (esSeparadorDecimal && (txtPrecio.Text.Contains(",") || txtPrecio.Text.Contains(".")))
            {
                e.Handled = true;
            }
        }
    }
}