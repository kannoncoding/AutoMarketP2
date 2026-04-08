/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el CRUD de la entidad Vehiculo en SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class VehiculoDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public VehiculoDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public VehiculoDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public void Insertar(Vehiculo vehiculo)
        {
            if (vehiculo == null)
                throw new ArgumentNullException(nameof(vehiculo));

            if (ExisteId(vehiculo.IdVehiculo))
                throw new InvalidOperationException("Ya existe un vehículo con ese id.");

            if (!ExisteCategoria(vehiculo.Categoria.IdCategoria))
                throw new InvalidOperationException("La categoría asociada no existe.");

            const string sql = @"
INSERT INTO Vehiculo
(IdVehiculo, Marca, Modelo, Anio, Precio, IdCategoria, Estado)
VALUES
(@IdVehiculo, @Marca, @Modelo, @Anio, @Precio, @IdCategoria, @Estado);";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdVehiculo", vehiculo.IdVehiculo);
            cmd.Parameters.AddWithValue("@Marca", vehiculo.Marca);
            cmd.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
            cmd.Parameters.AddWithValue("@Anio", vehiculo.Anio);
            cmd.Parameters.AddWithValue("@Precio", vehiculo.Precio);
            cmd.Parameters.AddWithValue("@IdCategoria", vehiculo.Categoria.IdCategoria);
            cmd.Parameters.AddWithValue("@Estado", vehiculo.Estado);

            cn.Open();
            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("No se pudo insertar el vehículo.");
        }

        public void Actualizar(Vehiculo vehiculo)
        {
            if (vehiculo == null)
                throw new ArgumentNullException(nameof(vehiculo));

            if (!ExisteId(vehiculo.IdVehiculo))
                throw new InvalidOperationException("El vehículo no existe.");

            if (!ExisteCategoria(vehiculo.Categoria.IdCategoria))
                throw new InvalidOperationException("La categoría asociada no existe.");

            const string sql = @"
UPDATE Vehiculo
SET Marca=@Marca,
    Modelo=@Modelo,
    Anio=@Anio,
    Precio=@Precio,
    IdCategoria=@IdCategoria,
    Estado=@Estado
WHERE IdVehiculo=@IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdVehiculo", vehiculo.IdVehiculo);
            cmd.Parameters.AddWithValue("@Marca", vehiculo.Marca);
            cmd.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
            cmd.Parameters.AddWithValue("@Anio", vehiculo.Anio);
            cmd.Parameters.AddWithValue("@Precio", vehiculo.Precio);
            cmd.Parameters.AddWithValue("@IdCategoria", vehiculo.Categoria.IdCategoria);
            cmd.Parameters.AddWithValue("@Estado", vehiculo.Estado);

            cn.Open();
            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("No se pudo actualizar el vehículo.");
        }

        public void Eliminar(int idVehiculo)
        {
            if (idVehiculo <= 0)
                throw new ArgumentException("Id inválido.");

            if (!ExisteId(idVehiculo))
                throw new InvalidOperationException("El vehículo no existe.");

            const string sql = "DELETE FROM Vehiculo WHERE IdVehiculo=@IdVehiculo";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("No se pudo eliminar.");
        }

        public Vehiculo? ObtenerPorId(int idVehiculo)
        {
            const string sql = @"
SELECT v.*, c.NombreCategoria, c.Descripcion
FROM Vehiculo v
INNER JOIN CategoriaVehiculo c ON v.IdCategoria = c.IdCategoria
WHERE v.IdVehiculo=@IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                var categoria = new CategoriaVehiculo(
                    Convert.ToInt32(dr["IdCategoria"]),
                    dr["NombreCategoria"].ToString() ?? "",
                    dr["Descripcion"].ToString() ?? ""
                );

                return new Vehiculo(
                    Convert.ToInt32(dr["IdVehiculo"]),
                    dr["Marca"].ToString() ?? "",
                    dr["Modelo"].ToString() ?? "",
                    Convert.ToInt32(dr["Anio"]),
                    Convert.ToDecimal(dr["Precio"]),
                    categoria,
                    Convert.ToChar(dr["Estado"])
                );
            }

            return null;
        }

        public List<Vehiculo> ObtenerTodos()
        {
            List<Vehiculo> lista = new List<Vehiculo>();

            const string sql = @"
SELECT v.*, c.NombreCategoria, c.Descripcion
FROM Vehiculo v
INNER JOIN CategoriaVehiculo c ON v.IdCategoria = c.IdCategoria
ORDER BY v.IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var categoria = new CategoriaVehiculo(
                    Convert.ToInt32(dr["IdCategoria"]),
                    dr["NombreCategoria"].ToString() ?? "",
                    dr["Descripcion"].ToString() ?? ""
                );

                var vehiculo = new Vehiculo(
                    Convert.ToInt32(dr["IdVehiculo"]),
                    dr["Marca"].ToString() ?? "",
                    dr["Modelo"].ToString() ?? "",
                    Convert.ToInt32(dr["Anio"]),
                    Convert.ToDecimal(dr["Precio"]),
                    categoria,
                    Convert.ToChar(dr["Estado"])
                );

                lista.Add(vehiculo);
            }

            return lista;
        }

        public bool ExisteId(int idVehiculo)
        {
            const string sql = "SELECT COUNT(1) FROM Vehiculo WHERE IdVehiculo=@IdVehiculo";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private bool ExisteCategoria(int idCategoria)
        {
            const string sql = "SELECT COUNT(1) FROM CategoriaVehiculo WHERE IdCategoria=@IdCategoria";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }
}