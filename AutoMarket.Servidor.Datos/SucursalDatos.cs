/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el CRUD de la entidad Sucursal en SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class SucursalDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public SucursalDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public SucursalDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public void Insertar(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException(nameof(sucursal), "La sucursal es obligatoria.");
            }

            if (ExisteId(sucursal.IdSucursal))
            {
                throw new InvalidOperationException("Ya existe una sucursal registrada con el mismo id.");
            }

            if (!ExisteVendedor(sucursal.VendedorEncargado.IdVendedor))
            {
                throw new InvalidOperationException("El vendedor encargado asociado no existe.");
            }

            const string consultaSql = @"
INSERT INTO Sucursal
(
    IdSucursal,
    Nombre,
    Direccion,
    Telefono,
    IdVendedor,
    Activo
)
VALUES
(
    @IdSucursal,
    @Nombre,
    @Direccion,
    @Telefono,
    @IdVendedor,
    @Activo
);";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", sucursal.IdSucursal);
                comando.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                comando.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                comando.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                comando.Parameters.AddWithValue("@IdVendedor", sucursal.VendedorEncargado.IdVendedor);
                comando.Parameters.AddWithValue("@Activo", sucursal.Activo);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible insertar la sucursal.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al insertar la sucursal en la base de datos.", ex);
                }
            }
        }

        public void Actualizar(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException(nameof(sucursal), "La sucursal es obligatoria.");
            }

            if (!ExisteId(sucursal.IdSucursal))
            {
                throw new InvalidOperationException("No existe una sucursal registrada con el id indicado.");
            }

            if (!ExisteVendedor(sucursal.VendedorEncargado.IdVendedor))
            {
                throw new InvalidOperationException("El vendedor encargado asociado no existe.");
            }

            const string consultaSql = @"
UPDATE Sucursal
SET
    Nombre = @Nombre,
    Direccion = @Direccion,
    Telefono = @Telefono,
    IdVendedor = @IdVendedor,
    Activo = @Activo
WHERE
    IdSucursal = @IdSucursal;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", sucursal.IdSucursal);
                comando.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                comando.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                comando.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                comando.Parameters.AddWithValue("@IdVendedor", sucursal.VendedorEncargado.IdVendedor);
                comando.Parameters.AddWithValue("@Activo", sucursal.Activo);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible actualizar la sucursal.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al actualizar la sucursal en la base de datos.", ex);
                }
            }
        }

        public void Eliminar(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            if (!ExisteId(idSucursal))
            {
                throw new InvalidOperationException("No existe una sucursal registrada con el id indicado.");
            }

            const string consultaSql = @"
DELETE FROM Sucursal
WHERE IdSucursal = @IdSucursal;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible eliminar la sucursal.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al eliminar la sucursal en la base de datos.", ex);
                }
            }
        }

        public Sucursal? ObtenerPorId(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            const string consultaSql = @"
SELECT
    s.IdSucursal,
    s.Nombre,
    s.Direccion,
    s.Telefono,
    s.Activo,
    v.IdVendedor,
    v.Identificacion,
    v.NombreCompleto,
    v.FechaNacimiento,
    v.FechaIngreso,
    v.Telefono AS TelefonoVendedor
FROM Sucursal s
INNER JOIN Vendedor v ON s.IdVendedor = v.IdVendedor
WHERE s.IdSucursal = @IdSucursal;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            Vendedor vendedor = new Vendedor(
                                Convert.ToInt32(lector["IdVendedor"]),
                                Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                                Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                                Convert.ToDateTime(lector["FechaNacimiento"]),
                                Convert.ToDateTime(lector["FechaIngreso"]),
                                Convert.ToString(lector["TelefonoVendedor"]) ?? string.Empty
                            );

                            return new Sucursal(
                                Convert.ToInt32(lector["IdSucursal"]),
                                Convert.ToString(lector["Nombre"]) ?? string.Empty,
                                Convert.ToString(lector["Direccion"]) ?? string.Empty,
                                Convert.ToString(lector["Telefono"]) ?? string.Empty,
                                vendedor,
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar la sucursal en la base de datos.", ex);
                }
            }

            return null;
        }

        public List<Sucursal> ObtenerTodos()
        {
            List<Sucursal> sucursales = new List<Sucursal>();

            const string consultaSql = @"
SELECT
    s.IdSucursal,
    s.Nombre,
    s.Direccion,
    s.Telefono,
    s.Activo,
    v.IdVendedor,
    v.Identificacion,
    v.NombreCompleto,
    v.FechaNacimiento,
    v.FechaIngreso,
    v.Telefono AS TelefonoVendedor
FROM Sucursal s
INNER JOIN Vendedor v ON s.IdVendedor = v.IdVendedor
ORDER BY s.IdSucursal;";

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
                                Convert.ToString(lector["TelefonoVendedor"]) ?? string.Empty
                            );

                            Sucursal sucursal = new Sucursal(
                                Convert.ToInt32(lector["IdSucursal"]),
                                Convert.ToString(lector["Nombre"]) ?? string.Empty,
                                Convert.ToString(lector["Direccion"]) ?? string.Empty,
                                Convert.ToString(lector["Telefono"]) ?? string.Empty,
                                vendedor,
                                Convert.ToBoolean(lector["Activo"])
                            );

                            sucursales.Add(sucursal);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar las sucursales en la base de datos.", ex);
                }
            }

            return sucursales;
        }

        public List<Sucursal> ObtenerActivas()
        {
            List<Sucursal> sucursales = new List<Sucursal>();

            const string consultaSql = @"
SELECT
    s.IdSucursal,
    s.Nombre,
    s.Direccion,
    s.Telefono,
    s.Activo,
    v.IdVendedor,
    v.Identificacion,
    v.NombreCompleto,
    v.FechaNacimiento,
    v.FechaIngreso,
    v.Telefono AS TelefonoVendedor
FROM Sucursal s
INNER JOIN Vendedor v ON s.IdVendedor = v.IdVendedor
WHERE s.Activo = 1
ORDER BY s.IdSucursal;";

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
                                Convert.ToString(lector["TelefonoVendedor"]) ?? string.Empty
                            );

                            Sucursal sucursal = new Sucursal(
                                Convert.ToInt32(lector["IdSucursal"]),
                                Convert.ToString(lector["Nombre"]) ?? string.Empty,
                                Convert.ToString(lector["Direccion"]) ?? string.Empty,
                                Convert.ToString(lector["Telefono"]) ?? string.Empty,
                                vendedor,
                                Convert.ToBoolean(lector["Activo"])
                            );

                            sucursales.Add(sucursal);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar las sucursales activas en la base de datos.", ex);
                }
            }

            return sucursales;
        }

        public bool ExisteId(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                return false;
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Sucursal
WHERE IdSucursal = @IdSucursal;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia del id de la sucursal.", ex);
                }
            }
        }

        public bool ExisteVendedor(int idVendedor)
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
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia del vendedor encargado.", ex);
                }
            }
        }
    }
}