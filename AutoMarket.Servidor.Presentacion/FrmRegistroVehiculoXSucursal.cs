/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para registrar asociaciones de vehículos por sucursal directamente en la capa lógica del servidor y persistirlas en SQL Server.
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
    public partial class FrmRegistroVehiculoXSucursal : Form
    {
        private readonly VehiculoxSucursalLogica _vehiculoxSucursalLogica;
        private readonly SucursalLogica _sucursalLogica;
        private readonly VehiculoLogica _vehiculoLogica;

        public FrmRegistroVehiculoXSucursal()
            : this(new VehiculoxSucursalLogica(), new SucursalLogica(), new VehiculoLogica())
        {
        }

        public FrmRegistroVehiculoXSucursal(
            VehiculoxSucursalLogica vehiculoxSucursalLogica,
            SucursalLogica sucursalLogica,
            VehiculoLogica vehiculoLogica)
        {
            _vehiculoxSucursalLogica = vehiculoxSucursalLogica ?? throw new ArgumentNullException(nameof(vehiculoxSucursalLogica));
            _sucursalLogica = sucursalLogica ?? throw new ArgumentNullException(nameof(sucursalLogica));
            _vehiculoLogica = vehiculoLogica ?? throw new ArgumentNullException(nameof(vehiculoLogica));

            InitializeComponent();
            ConfigurarFormulario();
        }

        private void FrmRegistroVehiculoXSucursal_Load(object sender, EventArgs e)
        {
            try
            {
                CargarSucursalesActivas();
                CargarVehiculos();
                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar el formulario de vehículo por sucursal.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Registro de Vehículo por Sucursal";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.SystemColors.Control;
        }

        private void CargarSucursalesActivas()
        {
            List<Sucursal> sucursales = _sucursalLogica.ObtenerActivas();

            cmbSucursal.DataSource = null;
            cmbSucursal.DisplayMember = nameof(Sucursal.Nombre);
            cmbSucursal.ValueMember = nameof(Sucursal.IdSucursal);

            if (sucursales.Count == 0)
            {
                cmbSucursal.Enabled = false;
                btnGuardar.Enabled = false;

                MessageBox.Show(
                    "No hay sucursales activas registradas en la base de datos. Debe registrar al menos una sucursal activa antes de asociar vehículos.",
                    "Sucursales activas requeridas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            cmbSucursal.DataSource = sucursales;
            cmbSucursal.SelectedIndex = -1;
            cmbSucursal.Enabled = true;
        }

        private void CargarVehiculos()
        {
            List<Vehiculo> vehiculos = _vehiculoLogica.ObtenerTodos();

            cmbVehiculo.DataSource = null;
            cmbVehiculo.DisplayMember = nameof(Vehiculo.Modelo);
            cmbVehiculo.ValueMember = nameof(Vehiculo.IdVehiculo);

            if (vehiculos.Count == 0)
            {
                cmbVehiculo.Enabled = false;
                btnGuardar.Enabled = false;

                MessageBox.Show(
                    "No hay vehículos registrados en la base de datos. Debe registrar al menos un vehículo antes de asociarlo a una sucursal.",
                    "Vehículos requeridos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            cmbVehiculo.DataSource = vehiculos;
            cmbVehiculo.SelectedIndex = -1;
            cmbVehiculo.Enabled = true;
        }

        private void PrepararFormularioNuevoRegistro()
        {
            if (cmbSucursal.Items.Count > 0)
            {
                cmbSucursal.SelectedIndex = -1;
            }

            if (cmbVehiculo.Items.Count > 0)
            {
                cmbVehiculo.SelectedIndex = -1;
            }

            txtCantidad.Text = "0";
            errorProviderFormulario.Clear();

            if (cmbSucursal.Enabled)
            {
                cmbSucursal.Focus();
            }
        }

        private bool ValidarDatos()
        {
            errorProviderFormulario.Clear();

            bool esValido = true;

            if (cmbSucursal.SelectedItem == null)
            {
                errorProviderFormulario.SetError(cmbSucursal, "Debe seleccionar una sucursal activa.");
                esValido = false;
            }

            if (cmbVehiculo.SelectedItem == null)
            {
                errorProviderFormulario.SetError(cmbVehiculo, "Debe seleccionar un vehículo.");
                esValido = false;
            }

            string textoCantidad = txtCantidad.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoCantidad))
            {
                errorProviderFormulario.SetError(txtCantidad, "Debe ingresar la cantidad.");
                esValido = false;
            }
            else if (!int.TryParse(textoCantidad, out int cantidad))
            {
                errorProviderFormulario.SetError(txtCantidad, "Debe ingresar una cantidad válida.");
                esValido = false;
            }
            else if (cantidad < 0)
            {
                errorProviderFormulario.SetError(txtCantidad, "La cantidad no puede ser menor que cero.");
                esValido = false;
            }

            return esValido;
        }

        private VehiculoxSucursal ConstruirEntidadDesdeFormulario()
        {
            Sucursal? sucursal = cmbSucursal.SelectedItem as Sucursal;
            if (sucursal == null)
            {
                throw new InvalidOperationException("Debe seleccionar una sucursal válida.");
            }

            Vehiculo? vehiculo = cmbVehiculo.SelectedItem as Vehiculo;
            if (vehiculo == null)
            {
                throw new InvalidOperationException("Debe seleccionar un vehículo válido.");
            }

            int cantidad = Convert.ToInt32(txtCantidad.Text.Trim());

            return new VehiculoxSucursal(sucursal, vehiculo, cantidad);
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

                VehiculoxSucursal vehiculoxSucursal = ConstruirEntidadDesdeFormulario();
                _vehiculoxSucursalLogica.Registrar(vehiculoxSucursal);

                MessageBox.Show(
                    "La asociación de vehículo por sucursal fue registrada correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarSucursalesActivas();
                CargarVehiculos();
                PrepararFormularioNuevoRegistro();
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar la asociación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error inesperado al registrar la asociación de vehículo por sucursal.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
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
                    "¿Desea cerrar el formulario de registro de vehículo por sucursal?",
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

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
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