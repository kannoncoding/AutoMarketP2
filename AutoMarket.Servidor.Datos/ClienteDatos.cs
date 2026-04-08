/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el CRUD de la entidad Cliente en SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class ClienteDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public ClienteDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public ClienteDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public void Insertar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente es obligatorio.");
            }

            if (ExisteId(cliente.IdCliente))
            {
                throw new InvalidOperationException("Ya existe un cliente registrado con el mismo id.");
            }

            if (ExisteIdentificacion(cliente.Identificacion))
            {
                throw new InvalidOperationException("Ya existe un cliente registrado con la misma identificación.");
            }

            const string consultaSql = @"
INSERT INTO Cliente
(
    IdCliente,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaRegistro,
    Activo
)
VALUES
(
    @IdCliente,
    @Identificacion,
    @NombreCompleto,
    @FechaNacimiento,
    @FechaRegistro,
    @Activo
);";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                comando.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                comando.Parameters.AddWithValue("@NombreCompleto", cliente.NombreCompleto);
                comando.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                comando.Parameters.AddWithValue("@FechaRegistro", cliente.FechaRegistro);
                comando.Parameters.AddWithValue("@Activo", cliente.Activo);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible insertar el cliente.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al insertar el cliente en la base de datos.", ex);
                }
            }
        }

        public void Actualizar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente es obligatorio.");
            }

            if (!ExisteId(cliente.IdCliente))
            {
                throw new InvalidOperationException("No existe un cliente registrado con el id indicado.");
            }

            if (ExisteIdentificacionEnOtroCliente(cliente.IdCliente, cliente.Identificacion))
            {
                throw new InvalidOperationException("Ya existe otro cliente registrado con la misma identificación.");
            }

            const string consultaSql = @"
UPDATE Cliente
SET
    Identificacion = @Identificacion,
    NombreCompleto = @NombreCompleto,
    FechaNacimiento = @FechaNacimiento,
    FechaRegistro = @FechaRegistro,
    Activo = @Activo
WHERE
    IdCliente = @IdCliente;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                comando.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                comando.Parameters.AddWithValue("@NombreCompleto", cliente.NombreCompleto);
                comando.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                comando.Parameters.AddWithValue("@FechaRegistro", cliente.FechaRegistro);
                comando.Parameters.AddWithValue("@Activo", cliente.Activo);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible actualizar el cliente.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al actualizar el cliente en la base de datos.", ex);
                }
            }
        }

        public void Eliminar(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            if (!ExisteId(idCliente))
            {
                throw new InvalidOperationException("No existe un cliente registrado con el id indicado.");
            }

            const string consultaSql = @"
DELETE FROM Cliente
WHERE IdCliente = @IdCliente;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible eliminar el cliente.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al eliminar el cliente en la base de datos.", ex);
                }
            }
        }

        public Cliente? ObtenerPorId(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            const string consultaSql = @"
SELECT
    IdCliente,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaRegistro,
    Activo
FROM Cliente
WHERE IdCliente = @IdCliente;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Cliente(
                                Convert.ToInt32(lector["IdCliente"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaRegistro"]),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar el cliente en la base de datos.", ex);
                }
            }

            return null;
        }

        public Cliente? ObtenerPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            const string consultaSql = @"
SELECT
    IdCliente,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaRegistro,
    Activo
FROM Cliente
WHERE Identificacion = @Identificacion;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@Identificacion", identificacionNormalizada);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Cliente(
                                Convert.ToInt32(lector["IdCliente"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaRegistro"]),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar el cliente por identificación en la base de datos.", ex);
                }
            }

            return null;
        }

        public Cliente? ObtenerClienteActivoPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            const string consultaSql = @"
SELECT
    IdCliente,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaRegistro,
    Activo
FROM Cliente
WHERE Identificacion = @Identificacion
AND Activo = 1;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@Identificacion", identificacionNormalizada);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Cliente(
                                Convert.ToInt32(lector["IdCliente"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaRegistro"]),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar el cliente activo en la base de datos.", ex);
                }
            }

            return null;
        }

        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> clientes = new List<Cliente>();

            const string consultaSql = @"
SELECT
    IdCliente,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaRegistro,
    Activo
FROM Cliente
ORDER BY IdCliente;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Cliente cliente = new Cliente(
                                Convert.ToInt32(lector["IdCliente"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaRegistro"]),
                                Convert.ToBoolean(lector["Activo"])
                            );

                            clientes.Add(cliente);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar los clientes en la base de datos.", ex);
                }
            }

            return clientes;
        }

        public bool ExisteId(int idCliente)
        {
            if (idCliente <= 0)
            {
                return false;
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Cliente
WHERE IdCliente = @IdCliente;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia del id del cliente.", ex);
                }
            }
        }

        public bool ExisteIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                return false;
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Cliente
WHERE Identificacion = @Identificacion;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@Identificacion", identificacionNormalizada);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia de la identificación del cliente.", ex);
                }
            }
        }

        public bool ExisteIdentificacionEnOtroCliente(int idCliente, string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.");
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Cliente
WHERE Identificacion = @Identificacion
AND IdCliente <> @IdCliente;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@Identificacion", identificacionNormalizada);
                comando.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la identificación duplicada del cliente.", ex);
                }
            }
        }
    }
}