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

                MessageBox.Show(
                    "Conexión con SQL Server establecida correctamente.",
                    "AutoMarket",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Application.Run(new Form1());
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