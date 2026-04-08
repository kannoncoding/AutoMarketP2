/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso centralizado a la configuración y creación de conexiones hacia SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class ConexionSqlServer
    {
        private readonly string _cadenaConexion;

        public string CadenaConexion
        {
            get => _cadenaConexion;
        }

        public ConexionSqlServer()
        {
            _cadenaConexion = ConfigurationManager.ConnectionStrings["AutoMarketBD"]?.ConnectionString ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_cadenaConexion))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión 'AutoMarketBD' en el archivo de configuración.");
            }
        }

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }

        public void ProbarConexion()
        {
            try
            {
                using (SqlConnection conexion = CrearConexion())
                {
                    conexion.Open();
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("No fue posible establecer la conexión con la base de datos SQL Server.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error inesperado al intentar conectarse a la base de datos.", ex);
            }
        }
    }
}