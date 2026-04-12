/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #1
Descripción: Formulario principal administrativo del servidor para iniciar o detener el servicio TCP, visualizar el estado general, registrar eventos en bitácora y navegar hacia los módulos de mantenimiento y consulta del sistema AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoMarket.Servidor.Presentacion
{
    public partial class FrmServidorPrincipal : Form
    {
        public event EventHandler SolicitudIniciarServidor;
        public event EventHandler SolicitudDetenerServidor;

        public FrmServidorPrincipal()
        {
            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarEstadoInicial();
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Servidor Principal";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1300, 780);
        }

        private void ConfigurarEstadoInicial()
        {
            ActualizarEstadoServidor(false, "🔴 Servidor detenido");
            AgregarEventoBitacora("Sistema listo. El servidor aún no ha sido iniciado.");
        }

        public void ActualizarEstadoServidor(bool servidorActivo, string mensajeEstado)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, string>(ActualizarEstadoServidor), servidorActivo, mensajeEstado);
                return;
            }

            lblEstadoValor.Text = mensajeEstado;
            lblEstadoValor.ForeColor = servidorActivo ? Color.ForestGreen : Color.Firebrick;

            btnIniciarServidor.Enabled = !servidorActivo;
            btnDetenerServidor.Enabled = servidorActivo;
        }

        public void AgregarEventoBitacora(string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AgregarEventoBitacora), mensaje);
                return;
            }

            string fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txtBitacora.AppendText($"[{fechaHora}] {mensaje}{Environment.NewLine}");
        }

        public void LimpiarBitacora()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LimpiarBitacora));
                return;
            }

            txtBitacora.Clear();
            AgregarEventoBitacora("La bitácora fue limpiada por el administrador.");
        }

        public void EstablecerCantidadConexiones(int cantidadConexiones)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(EstablecerCantidadConexiones), cantidadConexiones);
                return;
            }

            lblConexionesValor.Text = cantidadConexiones.ToString();
        }

        public void EstablecerDireccionEscucha(string direccionIp, int puerto)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, int>(EstablecerDireccionEscucha), direccionIp, puerto);
                return;
            }

            lblDireccionValor.Text = direccionIp;
            lblPuertoValor.Text = puerto.ToString();
        }

        private void btnIniciarServidor_Click(object sender, EventArgs e)
        {
            try
            {
                SolicitudIniciarServidor?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al solicitar el inicio del servidor.\n\nDetalle: {ex.Message}",
                    "Error al iniciar servidor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                AgregarEventoBitacora($"Error al solicitar inicio del servidor: {ex.Message}");
            }
        }

        private void btnDetenerServidor_Click(object sender, EventArgs e)
        {
            try
            {
                SolicitudDetenerServidor?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al solicitar la detención del servidor.\n\nDetalle: {ex.Message}",
                    "Error al detener servidor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                AgregarEventoBitacora($"Error al solicitar detención del servidor: {ex.Message}");
            }
        }

        private void btnLimpiarBitacora_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea limpiar completamente la bitácora de eventos?",
                    "Confirmar limpieza",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    LimpiarBitacora();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al limpiar la bitácora.\n\nDetalle: {ex.Message}",
                    "Error de bitácora",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar la aplicación del servidor?",
                    "Salir del sistema",
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
                    $"Ocurrió un error al intentar cerrar la aplicación.\n\nDetalle: {ex.Message}",
                    "Error al salir",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRegistroCategoriaVehiculo_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Registro de Categoría de Vehículo");
        }

        private void btnRegistroVehiculo_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Registro de Vehículo");
        }

        private void btnRegistroVendedor_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Registro de Vendedor");
        }

        private void btnRegistroSucursal_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Registro de Sucursal");
        }

        private void btnRegistroCliente_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Registro de Cliente");
        }

        private void btnRegistroVehiculoXSucursal_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Registro de Vehículo por Sucursal");
        }

        private void btnConsultaSucursal_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Sucursal");
        }

        private void btnConsultaVehiculo_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Vehículo");
        }

        private void btnConsultaVehiculoXSucursal_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Vehículo por Sucursal");
        }

        private void btnConsultaCategoriaVehiculo_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Categoría de Vehículo");
        }

        private void btnConsultaVendedor_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Vendedor");
        }

        private void btnConsultaCliente_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Cliente");
        }

        private void btnConsultaVenta_Click(object sender, EventArgs e)
        {
            AbrirFormularioModulo("Consulta de Venta");
        }

        private void AbrirFormularioModulo(string nombreModulo)
        {
            try
            {
                MessageBox.Show(
                    $"Más adelante aquí se abrirá el módulo:\n\n{nombreModulo}",
                    "Módulo en preparación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                AgregarEventoBitacora($"El administrador abrió el acceso al módulo: {nombreModulo}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al intentar abrir el módulo.\n\nDetalle: {ex.Message}",
                    "Error al abrir módulo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FrmServidorPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (btnDetenerServidor.Enabled)
                {
                    DialogResult resultado = MessageBox.Show(
                        "El servidor parece estar activo. ¿Desea cerrar la aplicación de todos modos?",
                        "Servidor en ejecución",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (resultado == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch
            {
                e.Cancel = false;
            }
        }
    }
}