/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el CRUD de la entidad CategoriaVehiculo en SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class CategoriaVehiculoDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public CategoriaVehiculoDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public CategoriaVehiculoDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public void Insertar(CategoriaVehiculo categoriaVehiculo)
        {
            if (categoriaVehiculo == null)
            {
                throw new ArgumentNullException(nameof(categoriaVehiculo), "La categoría de vehículo es obligatoria.");
            }

            if (ExisteId(categoriaVehiculo.IdCategoria))
            {
                throw new InvalidOperationException("Ya existe una categoría de vehículo registrada con el mismo id.");
            }

            const string consultaSql = @"
INSERT INTO CategoriaVehiculo
(
    IdCategoria,
    NombreCategoria,
    Descripcion
)
VALUES
(
    @IdCategoria,
    @NombreCategoria,
    @Descripcion
);";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", categoriaVehiculo.IdCategoria);
                comando.Parameters.AddWithValue("@NombreCategoria", categoriaVehiculo.NombreCategoria);
                comando.Parameters.AddWithValue("@Descripcion", categoriaVehiculo.Descripcion);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible insertar la categoría de vehículo.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al insertar la categoría de vehículo en la base de datos.", ex);
                }
            }
        }

        public void Actualizar(CategoriaVehiculo categoriaVehiculo)
        {
            if (categoriaVehiculo == null)
            {
                throw new ArgumentNullException(nameof(categoriaVehiculo), "La categoría de vehículo es obligatoria.");
            }

            if (!ExisteId(categoriaVehiculo.IdCategoria))
            {
                throw new InvalidOperationException("No existe una categoría de vehículo registrada con el id indicado.");
            }

            const string consultaSql = @"
UPDATE CategoriaVehiculo
SET
    NombreCategoria = @NombreCategoria,
    Descripcion = @Descripcion
WHERE
    IdCategoria = @IdCategoria;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", categoriaVehiculo.IdCategoria);
                comando.Parameters.AddWithValue("@NombreCategoria", categoriaVehiculo.NombreCategoria);
                comando.Parameters.AddWithValue("@Descripcion", categoriaVehiculo.Descripcion);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible actualizar la categoría de vehículo.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al actualizar la categoría de vehículo en la base de datos.", ex);
                }
            }
        }

        public void Eliminar(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                throw new ArgumentException("El id de la categoría debe ser mayor que cero.");
            }

            if (!ExisteId(idCategoria))
            {
                throw new InvalidOperationException("No existe una categoría de vehículo registrada con el id indicado.");
            }

            const string consultaSql = @"
DELETE FROM CategoriaVehiculo
WHERE IdCategoria = @IdCategoria;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", idCategoria);

                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new InvalidOperationException("No fue posible eliminar la categoría de vehículo.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al eliminar la categoría de vehículo en la base de datos.", ex);
                }
            }
        }

        public CategoriaVehiculo ObtenerPorId(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                throw new ArgumentException("El id de la categoría debe ser mayor que cero.");
            }

            const string consultaSql = @"
SELECT
    IdCategoria,
    NombreCategoria,
    Descripcion
FROM CategoriaVehiculo
WHERE IdCategoria = @IdCategoria;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", idCategoria);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new CategoriaVehiculo(
                                Convert.ToInt32(lector["IdCategoria"]),
                                Convert.ToString(lector["NombreCategoria"]) ?? string.Empty,
                                Convert.ToString(lector["Descripcion"]) ?? string.Empty
                            );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar la categoría de vehículo en la base de datos.", ex);
                }
            }

            return null!;
        }

        public List<CategoriaVehiculo> ObtenerTodos()
        {
            List<CategoriaVehiculo> categorias = new List<CategoriaVehiculo>();

            const string consultaSql = @"
SELECT
    IdCategoria,
    NombreCategoria,
    Descripcion
FROM CategoriaVehiculo
ORDER BY IdCategoria;";

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
                            CategoriaVehiculo categoriaVehiculo = new CategoriaVehiculo(
                                Convert.ToInt32(lector["IdCategoria"]),
                                Convert.ToString(lector["NombreCategoria"]) ?? string.Empty,
                                Convert.ToString(lector["Descripcion"]) ?? string.Empty
                            );

                            categorias.Add(categoriaVehiculo);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar las categorías de vehículo en la base de datos.", ex);
                }
            }

            return categorias;
        }

        public bool ExisteId(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                return false;
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM CategoriaVehiculo
WHERE IdCategoria = @IdCategoria;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCategoria", idCategoria);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia del id de la categoría de vehículo.", ex);
                }
            }
        }
    }
}