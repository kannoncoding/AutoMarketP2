/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el CRUD de la entidad Vendedor en SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class VendedorDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public VendedorDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public VendedorDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public void Insertar(Vendedor vendedor)
        {
            if (vendedor == null)
            {
                throw new ArgumentNullException(nameof(vendedor), "El vendedor es obligatorio.");
            }

            if (ExisteId(vendedor.IdVendedor))
            {
                throw new InvalidOperationException("Ya existe un vendedor registrado con el mismo id.");
            }

            if (ExisteIdentificacion(vendedor.Identificacion))
            {
                throw new InvalidOperationException("Ya existe un vendedor registrado con la misma identificación.");
            }

            const string consultaSql = @"
INSERT INTO Vendedor
(
    IdVendedor,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaIngreso,
    Telefono
)
VALUES
(
    @IdVendedor,
    @Identificacion,
    @NombreCompleto,
    @FechaNacimiento,
    @FechaIngreso,
    @Telefono
);";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVendedor", vendedor.IdVendedor);
                comando.Parameters.AddWithValue("@Identificacion", vendedor.Identificacion);
                comando.Parameters.AddWithValue("@NombreCompleto", vendedor.NombreCompleto);
                comando.Parameters.AddWithValue("@FechaNacimiento", vendedor.FechaNacimiento);
                comando.Parameters.AddWithValue("@FechaIngreso", vendedor.FechaIngreso);
                comando.Parameters.AddWithValue("@Telefono", vendedor.Telefono);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible insertar el vendedor.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al insertar el vendedor en la base de datos.", ex);
                }
            }
        }

        public void Actualizar(Vendedor vendedor)
        {
            if (vendedor == null)
            {
                throw new ArgumentNullException(nameof(vendedor), "El vendedor es obligatorio.");
            }

            if (!ExisteId(vendedor.IdVendedor))
            {
                throw new InvalidOperationException("No existe un vendedor registrado con el id indicado.");
            }

            if (ExisteIdentificacionEnOtroVendedor(vendedor.IdVendedor, vendedor.Identificacion))
            {
                throw new InvalidOperationException("Ya existe otro vendedor registrado con la misma identificación.");
            }

            const string consultaSql = @"
UPDATE Vendedor
SET
    Identificacion = @Identificacion,
    NombreCompleto = @NombreCompleto,
    FechaNacimiento = @FechaNacimiento,
    FechaIngreso = @FechaIngreso,
    Telefono = @Telefono
WHERE
    IdVendedor = @IdVendedor;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVendedor", vendedor.IdVendedor);
                comando.Parameters.AddWithValue("@Identificacion", vendedor.Identificacion);
                comando.Parameters.AddWithValue("@NombreCompleto", vendedor.NombreCompleto);
                comando.Parameters.AddWithValue("@FechaNacimiento", vendedor.FechaNacimiento);
                comando.Parameters.AddWithValue("@FechaIngreso", vendedor.FechaIngreso);
                comando.Parameters.AddWithValue("@Telefono", vendedor.Telefono);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible actualizar el vendedor.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al actualizar el vendedor en la base de datos.", ex);
                }
            }
        }

        public void Eliminar(int idVendedor)
        {
            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
            }

            if (!ExisteId(idVendedor))
            {
                throw new InvalidOperationException("No existe un vendedor registrado con el id indicado.");
            }

            const string consultaSql = @"
DELETE FROM Vendedor
WHERE IdVendedor = @IdVendedor;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVendedor", idVendedor);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible eliminar el vendedor.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al eliminar el vendedor en la base de datos.", ex);
                }
            }
        }

        public Vendedor? ObtenerPorId(int idVendedor)
        {
            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
            }

            const string consultaSql = @"
SELECT
    IdVendedor,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaIngreso,
    Telefono
FROM Vendedor
WHERE IdVendedor = @IdVendedor;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVendedor", idVendedor);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Vendedor(
                                Convert.ToInt32(lector["IdVendedor"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaIngreso"]),
                                Convert.ToString(lector["Telefono"]) ?? string.Empty
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar el vendedor en la base de datos.", ex);
                }
            }

            return null;
        }

        public Vendedor? ObtenerPorIdentificacion(string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor es obligatoria.");
            }

            const string consultaSql = @"
SELECT
    IdVendedor,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaIngreso,
    Telefono
FROM Vendedor
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
                            return new Vendedor(
                                Convert.ToInt32(lector["IdVendedor"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaIngreso"]),
                                Convert.ToString(lector["Telefono"]) ?? string.Empty
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar el vendedor por identificación en la base de datos.", ex);
                }
            }

            return null;
        }

        public List<Vendedor> ObtenerTodos()
        {
            List<Vendedor> vendedores = new List<Vendedor>();

            const string consultaSql = @"
SELECT
    IdVendedor,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaIngreso,
    Telefono
FROM Vendedor
ORDER BY IdVendedor;";

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
                            Vendedor vendedor = new Vendedor(
                                Convert.ToInt32(lector["IdVendedor"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaIngreso"]),
                                Convert.ToString(lector["Telefono"]) ?? string.Empty
                            );

                            vendedores.Add(vendedor);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar los vendedores en la base de datos.", ex);
                }
            }

            return vendedores;
        }

        public bool ExisteId(int idVendedor)
        {
            if (idVendedor <= 0)
            {
                return false;
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Vendedor
WHERE IdVendedor = @IdVendedor;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVendedor", idVendedor);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia del id del vendedor.", ex);
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
FROM Vendedor
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
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia de la identificación del vendedor.", ex);
                }
            }
        }

        public bool ExisteIdentificacionEnOtroVendedor(int idVendedor, string identificacion)
        {
            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;

            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.");
            }

            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor es obligatoria.");
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Vendedor
WHERE Identificacion = @Identificacion
AND IdVendedor <> @IdVendedor;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@Identificacion", identificacionNormalizada);
                comando.Parameters.AddWithValue("@IdVendedor", idVendedor);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la identificación duplicada del vendedor.", ex);
                }
            }
        }
    }
}