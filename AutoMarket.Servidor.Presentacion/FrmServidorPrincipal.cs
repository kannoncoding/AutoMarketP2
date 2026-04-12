/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #1
Descripción: Formulario principal administrativo del servidor para iniciar y detener el servicio TCP, visualizar el estado general, mostrar la bitácora de eventos y navegar a los módulos del sistema.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-09
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
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ConfigurarEstadoInicial()
        {
            ActualizarEstadoServidor(false);
            ActualizarCantidadClientes(0);
            RegistrarEvento("Sistema iniciado. Interfaz administrativa lista.");
        }

        public void ActualizarEstadoServidor(bool servidorActivo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(ActualizarEstadoServidor), servidorActivo);
                return;
            }

            lblEstadoValor.Text = servidorActivo ? "ACTIVO" : "DETENIDO";
            lblEstadoValor.ForeColor = servidorActivo ? Color.ForestGreen : Color.Firebrick;

            pnlIndicadorEstado.BackColor = servidorActivo ? Color.ForestGreen : Color.Firebrick;

            btnIniciarServidor.Enabled = !servidorActivo;
            btnDetenerServidor.Enabled = servidorActivo;
        }

        public void ActualizarCantidadClientes(int cantidadClientes)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(ActualizarCantidadClientes), cantidadClientes);
                return;
            }

            lblClientesValor.Text = cantidadClientes.ToString();
        }

        public void RegistrarEvento(string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(RegistrarEvento), mensaje);
                return;
            }

            string linea = string.Format(
                "[{0}] {1}",
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                mensaje);

            txtBitacora.AppendText(linea + Environment.NewLine);
        }

        public void LimpiarBitacora()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LimpiarBitacora));
                return;
            }

            txtBitacora.Clear();
            RegistrarEvento("Bitácora limpiada por el administrador.");
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
                    "Ocurrió un error al intentar iniciar el servidor.\n\n" + ex.Message,
                    "Error al iniciar servidor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RegistrarEvento("Error al iniciar el servidor: " + ex.Message);
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
                    "Ocurrió un error al intentar detener el servidor.\n\n" + ex.Message,
                    "Error al detener servidor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RegistrarEvento("Error al detener el servidor: " + ex.Message);
            }
        }

        private void btnLimpiarBitacora_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea limpiar la bitácora de eventos?",
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
                    "Ocurrió un error al limpiar la bitácora.\n\n" + ex.Message,
                    "Error al limpiar bitácora",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RegistrarEvento("Error al limpiar la bitácora: " + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar la aplicación del servidor?",
                    "Confirmar salida",
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
                    "Ocurrió un error al cerrar la aplicación.\n\n" + ex.Message,
                    "Error al salir",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RegistrarEvento("Error al cerrar la aplicación: " + ex.Message);
            }
        }

        private void btnRegistroCategoriaVehiculo_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Registro de Categoría de Vehículo");
        }

        private void btnRegistroVehiculo_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Registro de Vehículo");
        }

        private void btnRegistroVendedor_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Registro de Vendedor");
        }

        private void btnRegistroSucursal_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Registro de Sucursal");
        }

        private void btnRegistroCliente_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Registro de Cliente");
        }

        private void btnRegistroVehiculoXSucursal_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Registro de Vehículo por Sucursal");
        }

        private void btnConsultaSucursal_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Sucursal");
        }

        private void btnConsultaVehiculo_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Vehículo");
        }

        private void btnConsultaVehiculoXSucursal_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Vehículo por Sucursal");
        }

        private void btnConsultaCategoriaVehiculo_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Categoría de Vehículo");
        }

        private void btnConsultaVendedor_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Vendedor");
        }

        private void btnConsultaCliente_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Cliente");
        }

        private void btnConsultaVenta_Click(object sender, EventArgs e)
        {
            AbrirModuloNoImplementado("Consulta de Venta");
        }

        private void AbrirModuloNoImplementado(string nombreModulo)
        {
            try
            {
                MessageBox.Show(
                    "El módulo \"" + nombreModulo + "\" se conectará cuando construyamos ese formulario.",
                    "Módulo pendiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RegistrarEvento("Intento de acceso al módulo: " + nombreModulo + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al abrir el módulo.\n\n" + ex.Message,
                    "Error de navegación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RegistrarEvento("Error al abrir módulo " + nombreModulo + ": " + ex.Message);
            }
        }
    }
}