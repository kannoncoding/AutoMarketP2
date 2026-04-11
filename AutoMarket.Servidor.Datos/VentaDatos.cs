/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de acceso a datos para administrar el registro y consulta de ventas en SQL Server con control transaccional de inventario.
Estudiante: Jorge Arias M
Fecha de desarrollo: 2026-04-04
*/

using System;
using System.Collections.Generic;
using System.Data;
using AutoMarket.Entidades;
using Microsoft.Data.SqlClient;

namespace AutoMarket.Servidor.Datos
{
    public sealed class VentaDatos
    {
        private readonly ConexionSqlServer _conexionSqlServer;

        public VentaDatos()
        {
            _conexionSqlServer = new ConexionSqlServer();
        }

        public VentaDatos(ConexionSqlServer conexionSqlServer)
        {
            _conexionSqlServer = conexionSqlServer ?? throw new ArgumentNullException(nameof(conexionSqlServer));
        }

        public int RegistrarVenta(Venta venta)
        {
            if (venta == null)
            {
                throw new ArgumentNullException(nameof(venta), "La venta es obligatoria.");
            }

            if (venta.Cliente == null)
            {
                throw new ArgumentNullException(nameof(venta.Cliente), "El cliente de la venta es obligatorio.");
            }

            if (venta.Sucursal == null)
            {
                throw new ArgumentNullException(nameof(venta.Sucursal), "La sucursal de la venta es obligatoria.");
            }

            if (venta.Vehiculo == null)
            {
                throw new ArgumentNullException(nameof(venta.Vehiculo), "El vehículo de la venta es obligatorio.");
            }

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        Cliente cliente = ObtenerClienteActivoPorIdInterno(venta.Cliente.IdCliente, conexion, transaccion)
                            ?? throw new InvalidOperationException("El cliente no existe o no se encuentra activo.");

                        Sucursal sucursal = ObtenerSucursalActivaPorIdInterno(venta.Sucursal.IdSucursal, conexion, transaccion)
                            ?? throw new InvalidOperationException("La sucursal no existe o no se encuentra activa.");

                        Vehiculo vehiculo = ObtenerVehiculoPorIdInterno(venta.Vehiculo.IdVehiculo, conexion, transaccion)
                            ?? throw new InvalidOperationException("El vehículo no existe.");

                        int stockDisponible = ObtenerCantidadInventarioInterna(sucursal.IdSucursal, vehiculo.IdVehiculo, conexion, transaccion);

                        if (stockDisponible <= 0)
                        {
                            throw new InvalidOperationException("No hay stock disponible para el vehículo seleccionado en la sucursal indicada.");
                        }

                        decimal montoVenta = vehiculo.Precio;

                        int idVentaGenerado = InsertarVentaInterna(
                            cliente.IdCliente,
                            sucursal.IdSucursal,
                            vehiculo.IdVehiculo,
                            venta.FechaVenta,
                            montoVenta,
                            conexion,
                            transaccion);

                        DisminuirInventarioInterno(sucursal.IdSucursal, vehiculo.IdVehiculo, conexion, transaccion);

                        transaccion.Commit();

                        return idVentaGenerado;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public Venta? ObtenerPorId(int idVenta)
        {
            if (idVenta <= 0)
            {
                throw new ArgumentException("El id de la venta debe ser mayor que cero.");
            }

            const string consultaSql = @"
SELECT
    ve.IdVenta,
    ve.FechaVenta,
    ve.Monto,

    c.IdCliente,
    c.Identificacion AS ClienteIdentificacion,
    c.NombreCompleto AS ClienteNombreCompleto,
    c.FechaNacimiento AS ClienteFechaNacimiento,
    c.FechaRegistro AS ClienteFechaRegistro,
    c.Activo AS ClienteActivo,

    s.IdSucursal,
    s.Nombre AS SucursalNombre,
    s.Direccion AS SucursalDireccion,
    s.Telefono AS SucursalTelefono,
    s.Activo AS SucursalActivo,

    vd.IdVendedor,
    vd.Identificacion AS VendedorIdentificacion,
    vd.NombreCompleto AS VendedorNombreCompleto,
    vd.FechaNacimiento AS VendedorFechaNacimiento,
    vd.FechaIngreso AS VendedorFechaIngreso,
    vd.Telefono AS VendedorTelefono,

    vh.IdVehiculo,
    vh.Marca,
    vh.Modelo,
    vh.Ano,
    vh.Precio,
    vh.Estado,

    cv.IdCategoria,
    cv.NombreCategoria,
    cv.Descripcion
FROM Venta ve
INNER JOIN Cliente c ON ve.IdCliente = c.IdCliente
INNER JOIN Sucursal s ON ve.IdSucursal = s.IdSucursal
INNER JOIN Vendedor vd ON s.IdVendedor = vd.IdVendedor
INNER JOIN Vehiculo vh ON ve.IdVehiculo = vh.IdVehiculo
INNER JOIN CategoriaVehiculo cv ON vh.IdCategoria = cv.IdCategoria
WHERE ve.IdVenta = @IdVenta;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVenta", idVenta);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return ConstruirVentaDesdeReader(lector);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar la venta en la base de datos.", ex);
                }
            }

            return null;
        }

        public List<Venta> ObtenerTodos()
        {
            List<Venta> ventas = new List<Venta>();

            const string consultaSql = @"
SELECT
    ve.IdVenta,
    ve.FechaVenta,
    ve.Monto,

    c.IdCliente,
    c.Identificacion AS ClienteIdentificacion,
    c.NombreCompleto AS ClienteNombreCompleto,
    c.FechaNacimiento AS ClienteFechaNacimiento,
    c.FechaRegistro AS ClienteFechaRegistro,
    c.Activo AS ClienteActivo,

    s.IdSucursal,
    s.Nombre AS SucursalNombre,
    s.Direccion AS SucursalDireccion,
    s.Telefono AS SucursalTelefono,
    s.Activo AS SucursalActivo,

    vd.IdVendedor,
    vd.Identificacion AS VendedorIdentificacion,
    vd.NombreCompleto AS VendedorNombreCompleto,
    vd.FechaNacimiento AS VendedorFechaNacimiento,
    vd.FechaIngreso AS VendedorFechaIngreso,
    vd.Telefono AS VendedorTelefono,

    vh.IdVehiculo,
    vh.Marca,
    vh.Modelo,
    vh.Ano,
    vh.Precio,
    vh.Estado,

    cv.IdCategoria,
    cv.NombreCategoria,
    cv.Descripcion
FROM Venta ve
INNER JOIN Cliente c ON ve.IdCliente = c.IdCliente
INNER JOIN Sucursal s ON ve.IdSucursal = s.IdSucursal
INNER JOIN Vendedor vd ON s.IdVendedor = vd.IdVendedor
INNER JOIN Vehiculo vh ON ve.IdVehiculo = vh.IdVehiculo
INNER JOIN CategoriaVehiculo cv ON vh.IdCategoria = cv.IdCategoria
ORDER BY ve.IdVenta;";

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
                            ventas.Add(ConstruirVentaDesdeReader(lector));
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar las ventas en la base de datos.", ex);
                }
            }

            return ventas;
        }

        public List<Venta> ObtenerPorCliente(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.");
            }

            List<Venta> ventas = new List<Venta>();

            const string consultaSql = @"
SELECT
    ve.IdVenta,
    ve.FechaVenta,
    ve.Monto,

    c.IdCliente,
    c.Identificacion AS ClienteIdentificacion,
    c.NombreCompleto AS ClienteNombreCompleto,
    c.FechaNacimiento AS ClienteFechaNacimiento,
    c.FechaRegistro AS ClienteFechaRegistro,
    c.Activo AS ClienteActivo,

    s.IdSucursal,
    s.Nombre AS SucursalNombre,
    s.Direccion AS SucursalDireccion,
    s.Telefono AS SucursalTelefono,
    s.Activo AS SucursalActivo,

    vd.IdVendedor,
    vd.Identificacion AS VendedorIdentificacion,
    vd.NombreCompleto AS VendedorNombreCompleto,
    vd.FechaNacimiento AS VendedorFechaNacimiento,
    vd.FechaIngreso AS VendedorFechaIngreso,
    vd.Telefono AS VendedorTelefono,

    vh.IdVehiculo,
    vh.Marca,
    vh.Modelo,
    vh.Ano,
    vh.Precio,
    vh.Estado,

    cv.IdCategoria,
    cv.NombreCategoria,
    cv.Descripcion
FROM Venta ve
INNER JOIN Cliente c ON ve.IdCliente = c.IdCliente
INNER JOIN Sucursal s ON ve.IdSucursal = s.IdSucursal
INNER JOIN Vendedor vd ON s.IdVendedor = vd.IdVendedor
INNER JOIN Vehiculo vh ON ve.IdVehiculo = vh.IdVehiculo
INNER JOIN CategoriaVehiculo cv ON vh.IdCategoria = cv.IdCategoria
WHERE ve.IdCliente = @IdCliente
ORDER BY ve.IdVenta;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            ventas.Add(ConstruirVentaDesdeReader(lector));
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar las ventas del cliente en la base de datos.", ex);
                }
            }

            return ventas;
        }

        public List<Venta> ObtenerPorSucursal(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.");
            }

            List<Venta> ventas = new List<Venta>();

            const string consultaSql = @"
SELECT
    ve.IdVenta,
    ve.FechaVenta,
    ve.Monto,

    c.IdCliente,
    c.Identificacion AS ClienteIdentificacion,
    c.NombreCompleto AS ClienteNombreCompleto,
    c.FechaNacimiento AS ClienteFechaNacimiento,
    c.FechaRegistro AS ClienteFechaRegistro,
    c.Activo AS ClienteActivo,

    s.IdSucursal,
    s.Nombre AS SucursalNombre,
    s.Direccion AS SucursalDireccion,
    s.Telefono AS SucursalTelefono,
    s.Activo AS SucursalActivo,

    vd.IdVendedor,
    vd.Identificacion AS VendedorIdentificacion,
    vd.NombreCompleto AS VendedorNombreCompleto,
    vd.FechaNacimiento AS VendedorFechaNacimiento,
    vd.FechaIngreso AS VendedorFechaIngreso,
    vd.Telefono AS VendedorTelefono,

    vh.IdVehiculo,
    vh.Marca,
    vh.Modelo,
    vh.Ano,
    vh.Precio,
    vh.Estado,

    cv.IdCategoria,
    cv.NombreCategoria,
    cv.Descripcion
FROM Venta ve
INNER JOIN Cliente c ON ve.IdCliente = c.IdCliente
INNER JOIN Sucursal s ON ve.IdSucursal = s.IdSucursal
INNER JOIN Vendedor vd ON s.IdVendedor = vd.IdVendedor
INNER JOIN Vehiculo vh ON ve.IdVehiculo = vh.IdVehiculo
INNER JOIN CategoriaVehiculo cv ON vh.IdCategoria = cv.IdCategoria
WHERE ve.IdSucursal = @IdSucursal
ORDER BY ve.IdVenta;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            ventas.Add(ConstruirVentaDesdeReader(lector));
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al consultar las ventas de la sucursal en la base de datos.", ex);
                }
            }

            return ventas;
        }

        public bool ExisteId(int idVenta)
        {
            if (idVenta <= 0)
            {
                return false;
            }

            const string consultaSql = @"
SELECT COUNT(1)
FROM Venta
WHERE IdVenta = @IdVenta;";

            using (SqlConnection conexion = _conexionSqlServer.CrearConexion())
            using (SqlCommand comando = new SqlCommand(consultaSql, conexion))
            {
                comando.Parameters.AddWithValue("@IdVenta", idVenta);

                try
                {
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException("Ocurrió un error al validar la existencia del id de la venta.", ex);
                }
            }
        }

        private int InsertarVentaInterna(
            int idCliente,
            int idSucursal,
            int idVehiculo,
            DateTime fechaVenta,
            decimal monto,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string consultaSql = @"
INSERT INTO Venta
(
    IdCliente,
    IdSucursal,
    IdVehiculo,
    FechaVenta,
    Monto
)
VALUES
(
    @IdCliente,
    @IdSucursal,
    @IdVehiculo,
    @FechaVenta,
    @Monto
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand comando = new SqlCommand(consultaSql, conexion, transaccion))
            {
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;
                comando.Parameters.Add("@IdSucursal", SqlDbType.Int).Value = idSucursal;
                comando.Parameters.Add("@IdVehiculo", SqlDbType.Int).Value = idVehiculo;
                comando.Parameters.Add("@FechaVenta", SqlDbType.DateTime).Value = fechaVenta;

                SqlParameter parametroMonto = comando.Parameters.Add("@Monto", SqlDbType.Decimal);
                parametroMonto.Precision = 10;
                parametroMonto.Scale = 2;
                parametroMonto.Value = monto;

                object? resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    throw new InvalidOperationException("No fue posible obtener el id generado de la venta.");
                }

                return Convert.ToInt32(resultado);
            }
        }

        private void DisminuirInventarioInterno(
            int idSucursal,
            int idVehiculo,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string consultaSql = @"
UPDATE VehiculoxSucursal
SET Cantidad = Cantidad - 1
WHERE IdSucursal = @IdSucursal
AND IdVehiculo = @IdVehiculo
AND Cantidad > 0;";

            using (SqlCommand comando = new SqlCommand(consultaSql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);
                comando.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    throw new InvalidOperationException("No fue posible disminuir el inventario del vehículo porque no hay stock disponible.");
                }
            }
        }

        private int ObtenerCantidadInventarioInterna(
            int idSucursal,
            int idVehiculo,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string consultaSql = @"
SELECT Cantidad
FROM VehiculoxSucursal WITH (UPDLOCK, HOLDLOCK)
WHERE IdSucursal = @IdSucursal
AND IdVehiculo = @IdVehiculo;";

            using (SqlCommand comando = new SqlCommand(consultaSql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);
                comando.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

                object? resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(resultado);
            }
        }

        private Cliente? ObtenerClienteActivoPorIdInterno(
            int idCliente,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string consultaSql = @"
SELECT
    IdCliente,
    Identificacion,
    NombreCompleto,
    FechaNacimiento,
    FechaRegistro,
    Activo
FROM Cliente
WHERE IdCliente = @IdCliente
AND Activo = 1;";

            using (SqlCommand comando = new SqlCommand(consultaSql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdCliente", idCliente);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        Cliente cliente = new Cliente(
                            Convert.ToInt32(lector["IdCliente"]),
                            Convert.ToString(lector["Identificacion"]) ?? string.Empty,
                            Convert.ToString(lector["NombreCompleto"]) ?? string.Empty,
                            Convert.ToDateTime(lector["FechaNacimiento"]),
                            Convert.ToDateTime(lector["FechaRegistro"]),
                            Convert.ToBoolean(lector["Activo"])
                        );

                        lector.Close();
                        return cliente;
                    }
                }
            }

            return null;
        }

        private Sucursal? ObtenerSucursalActivaPorIdInterno(
            int idSucursal,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
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
WHERE s.IdSucursal = @IdSucursal
AND s.Activo = 1;";

            using (SqlCommand comando = new SqlCommand(consultaSql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdSucursal", idSucursal);

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

                        Sucursal sucursal = new Sucursal(
                            Convert.ToInt32(lector["IdSucursal"]),
                            Convert.ToString(lector["Nombre"]) ?? string.Empty,
                            Convert.ToString(lector["Direccion"]) ?? string.Empty,
                            Convert.ToString(lector["Telefono"]) ?? string.Empty,
                            vendedor,
                            Convert.ToBoolean(lector["Activo"])
                        );

                        lector.Close();
                        return sucursal;
                    }
                }
            }

            return null;
        }

        private Vehiculo? ObtenerVehiculoPorIdInterno(
            int idVehiculo,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string consultaSql = @"
SELECT
    vh.IdVehiculo,
    vh.Marca,
    vh.Modelo,
    vh.Ano,
    vh.Precio,
    vh.Estado,

    cv.IdCategoria,
    cv.NombreCategoria,
    cv.Descripcion
FROM Vehiculo vh
INNER JOIN CategoriaVehiculo cv ON vh.IdCategoria = cv.IdCategoria
WHERE vh.IdVehiculo = @IdVehiculo;";

            using (SqlCommand comando = new SqlCommand(consultaSql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        CategoriaVehiculo categoriaVehiculo = new CategoriaVehiculo(
                            Convert.ToInt32(lector["IdCategoria"]),
                            Convert.ToString(lector["NombreCategoria"]) ?? string.Empty,
                            Convert.ToString(lector["Descripcion"]) ?? string.Empty
                        );

                        Vehiculo vehiculo = new Vehiculo(
                            Convert.ToInt32(lector["IdVehiculo"]),
                            Convert.ToString(lector["Marca"]) ?? string.Empty,
                            Convert.ToString(lector["Modelo"]) ?? string.Empty,
                            Convert.ToInt32(lector["Ano"]),
                            Convert.ToDecimal(lector["Precio"]),
                            categoriaVehiculo,
                            Convert.ToChar(lector["Estado"])
                        );

                        lector.Close();
                        return vehiculo;
                    }
                }
            }

            return null;
        }

        private Venta ConstruirVentaDesdeReader(SqlDataReader lector)
        {
            Cliente cliente = new Cliente(
                Convert.ToInt32(lector["IdCliente"]),
                Convert.ToString(lector["ClienteIdentificacion"]) ?? string.Empty,
                Convert.ToString(lector["ClienteNombreCompleto"]) ?? string.Empty,
                Convert.ToDateTime(lector["ClienteFechaNacimiento"]),
                Convert.ToDateTime(lector["ClienteFechaRegistro"]),
                Convert.ToBoolean(lector["ClienteActivo"])
            );

            Vendedor vendedor = new Vendedor(
                Convert.ToInt32(lector["IdVendedor"]),
                Convert.ToString(lector["VendedorIdentificacion"]) ?? string.Empty,
                Convert.ToString(lector["VendedorNombreCompleto"]) ?? string.Empty,
                Convert.ToDateTime(lector["VendedorFechaNacimiento"]),
                Convert.ToDateTime(lector["VendedorFechaIngreso"]),
                Convert.ToString(lector["VendedorTelefono"]) ?? string.Empty
            );

            Sucursal sucursal = new Sucursal(
                Convert.ToInt32(lector["IdSucursal"]),
                Convert.ToString(lector["SucursalNombre"]) ?? string.Empty,
                Convert.ToString(lector["SucursalDireccion"]) ?? string.Empty,
                Convert.ToString(lector["SucursalTelefono"]) ?? string.Empty,
                vendedor,
                Convert.ToBoolean(lector["SucursalActivo"])
            );

            CategoriaVehiculo categoriaVehiculo = new CategoriaVehiculo(
                Convert.ToInt32(lector["IdCategoria"]),
                Convert.ToString(lector["NombreCategoria"]) ?? string.Empty,
                Convert.ToString(lector["Descripcion"]) ?? string.Empty
            );

            Vehiculo vehiculo = new Vehiculo(
                Convert.ToInt32(lector["IdVehiculo"]),
                Convert.ToString(lector["Marca"]) ?? string.Empty,
                Convert.ToString(lector["Modelo"]) ?? string.Empty,
                Convert.ToInt32(lector["Ano"]),
                Convert.ToDecimal(lector["Precio"]),
                categoriaVehiculo,
                Convert.ToChar(lector["Estado"])
            );

            return new Venta(
                Convert.ToInt32(lector["IdVenta"]),
                cliente,
                sucursal,
                vehiculo,
                Convert.ToDateTime(lector["FechaVenta"]),
                Convert.ToDecimal(lector["Monto"])
            );
        }
    }
}