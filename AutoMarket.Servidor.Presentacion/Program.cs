/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Punto de entrada principal de la aplicación servidor de AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Windows.Forms;
using AutoMarket.Servidor.Datos;

namespace AutoMarket.Servidor.Presentacion
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                ConexionSqlServer conexionSqlServer = new ConexionSqlServer();
                conexionSqlServer.ProbarConexion();

                FrmServidorPrincipal frmServidorPrincipal = new FrmServidorPrincipal();
                ControladorServidor controladorServidor = new ControladorServidor(frmServidorPrincipal);

                frmServidorPrincipal.FormClosed += (sender, e) =>
                {
                    controladorServidor.DetenerServidorSiEstaActivo();
                };

                Application.Run(frmServidorPrincipal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al iniciar la aplicación del servidor.{Environment.NewLine}{Environment.NewLine}Detalle: {ex.Message}",
                    "AutoMarket",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}