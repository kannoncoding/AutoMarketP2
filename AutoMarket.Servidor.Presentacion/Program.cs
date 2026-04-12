/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #1
Descripción: Punto de entrada principal de la aplicación servidor.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-09
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

                Application.Run(new FrmServidorPrincipal());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al conectar con la base de datos:{Environment.NewLine}{ex.Message}",
                    "AutoMarket",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}