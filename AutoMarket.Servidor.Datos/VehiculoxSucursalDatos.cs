/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el inventario de vehículos por sucursal en SQL Server.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class VehiculoxSucursalDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public VehiculoxSucursalDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public VehiculoxSucursalDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public void Insertar(VehiculoxSucursal relacion)
        {
            if (relacion == null)
                throw new ArgumentNullException(nameof(relacion));

            if (ExisteRelacion(relacion.Sucursal.IdSucursal, relacion.Vehiculo.IdVehiculo))
                throw new InvalidOperationException("La relación ya existe.");

            const string sql = @"
INSERT INTO VehiculoxSucursal
(IdSucursal, IdVehiculo, Cantidad)
VALUES
(@IdSucursal, @IdVehiculo, @Cantidad);";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", relacion.Sucursal.IdSucursal);
            cmd.Parameters.AddWithValue("@IdVehiculo", relacion.Vehiculo.IdVehiculo);
            cmd.Parameters.AddWithValue("@Cantidad", relacion.Cantidad);

            cn.Open();
            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("No se pudo insertar la relación.");
        }

        public void ActualizarCantidad(int idSucursal, int idVehiculo, int nuevaCantidad)
        {
            if (nuevaCantidad < 0)
                throw new ArgumentException("Cantidad inválida.");

            if (!ExisteRelacion(idSucursal, idVehiculo))
                throw new InvalidOperationException("La relación no existe.");

            const string sql = @"
UPDATE VehiculoxSucursal
SET Cantidad = @Cantidad
WHERE IdSucursal = @IdSucursal AND IdVehiculo = @IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@Cantidad", nuevaCantidad);
            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("No se pudo actualizar.");
        }

        public void Eliminar(int idSucursal, int idVehiculo)
        {
            const string sql = @"
DELETE FROM VehiculoxSucursal
WHERE IdSucursal=@IdSucursal AND IdVehiculo=@IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public VehiculoxSucursal? ObtenerRelacion(int idSucursal, int idVehiculo)
        {
            const string sql = @"
SELECT vs.Cantidad,
       s.IdSucursal, s.Nombre, s.Direccion, s.Telefono, s.Activo,
       v.IdVehiculo, v.Marca, v.Modelo, v.Anio, v.Precio, v.Estado,
       c.IdCategoria, c.NombreCategoria, c.Descripcion
FROM VehiculoxSucursal vs
INNER JOIN Sucursal s ON vs.IdSucursal = s.IdSucursal
INNER JOIN Vehiculo v ON vs.IdVehiculo = v.IdVehiculo
INNER JOIN CategoriaVehiculo c ON v.IdCategoria = c.IdCategoria
WHERE vs.IdSucursal=@IdSucursal AND vs.IdVehiculo=@IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
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

                var vehiculo = new Vehiculo(
                    Convert.ToInt32(dr["IdVehiculo"]),
                    dr["Marca"].ToString() ?? "",
                    dr["Modelo"].ToString() ?? "",
                    Convert.ToInt32(dr["Anio"]),
                    Convert.ToDecimal(dr["Precio"]),
                    categoria,
                    Convert.ToChar(dr["Estado"])
                );

                var sucursal = new Sucursal(
                    Convert.ToInt32(dr["IdSucursal"]),
                    dr["Nombre"].ToString() ?? "",
                    dr["Direccion"].ToString() ?? "",
                    dr["Telefono"].ToString() ?? "",
                    null!, // luego lo puedes mejorar si quieres traer vendedor
                    Convert.ToBoolean(dr["Activo"])
                );

                return new VehiculoxSucursal(
                    sucursal,
                    vehiculo,
                    Convert.ToInt32(dr["Cantidad"])
                );
            }

            return null;
        }

        public List<VehiculoxSucursal> ObtenerPorSucursal(int idSucursal)
        {
            List<VehiculoxSucursal> lista = new List<VehiculoxSucursal>();

            const string sql = @"
SELECT vs.Cantidad,
       v.IdVehiculo, v.Marca, v.Modelo, v.Anio, v.Precio, v.Estado,
       c.IdCategoria, c.NombreCategoria, c.Descripcion
FROM VehiculoxSucursal vs
INNER JOIN Vehiculo v ON vs.IdVehiculo = v.IdVehiculo
INNER JOIN CategoriaVehiculo c ON v.IdCategoria = c.IdCategoria
WHERE vs.IdSucursal=@IdSucursal;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);

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

                var sucursal = new Sucursal(
                    idSucursal,
                    "",
                    "",
                    "",
                    null!,
                    true
                );

                lista.Add(new VehiculoxSucursal(
                    sucursal,
                    vehiculo,
                    Convert.ToInt32(dr["Cantidad"])
                ));
            }

            return lista;
        }

        public bool ExisteRelacion(int idSucursal, int idVehiculo)
        {
            const string sql = @"
SELECT COUNT(1)
FROM VehiculoxSucursal
WHERE IdSucursal=@IdSucursal AND IdVehiculo=@IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public bool TieneStock(int idSucursal, int idVehiculo)
        {
            const string sql = @"
SELECT Cantidad
FROM VehiculoxSucursal
WHERE IdSucursal=@IdSucursal AND IdVehiculo=@IdVehiculo;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();
            object resultado = cmd.ExecuteScalar();

            if (resultado == null)
                return false;

            return Convert.ToInt32(resultado) > 0;
        }

        public void DisminuirStock(int idSucursal, int idVehiculo)
        {
            const string sql = @"
UPDATE VehiculoxSucursal
SET Cantidad = Cantidad - 1
WHERE IdSucursal=@IdSucursal AND IdVehiculo=@IdVehiculo AND Cantidad > 0;";

            using SqlConnection cn = _conexionSqlServer.CrearConexion();
            using SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
            cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

            cn.Open();

            if (cmd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("No hay stock disponible.");
        }
    }
}