using Microsoft.Data.SqlClient;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.Interfaces;
using System.Data;

namespace Proyecto_Botica_API.Repositorio.DAO
{
    public class ProductoDAO : IProducto
    {
        private readonly string _connection;
        public ProductoDAO()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cnx");
        }

        public string GuardarProducto(Producto producto)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarProducto", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.idCategoria);
                    cmd.Parameters.AddWithValue("@FechaFabricacion", producto.FechaFabricacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", producto.FechaVencimiento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@Imagen", producto.Imagen);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Producto guardado exitosamente." : "No se pudo guardar el producto.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al guardar el producto: " + ex.Message;
                }
            }
            return mensaje;
        }
        public string ActualizarProducto(Producto producto)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("sp_ActualizarProducto", cnx);
                    cmd.Parameters.AddWithValue("@IdProducto",producto.IdProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.idCategoria);
                    cmd.Parameters.AddWithValue("@FechaFabricacion", producto.FechaFabricacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", producto.FechaVencimiento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@Imagen", producto.Imagen);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Producto actualizado exitosamente." : "No se pudo actualizo el producto.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al actualizar el producto: " + ex.Message;
                }
            }
            return mensaje;
        }
        public IEnumerable<Producto> obtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarProductos", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    productos.Add(new Producto
                    {
                        IdProducto = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Descripcion = dr.GetString(2),
                        NombreCategoria = dr.GetString(3),
                        FechaFabricacion = dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4),
                        FechaVencimiento = dr.IsDBNull(5) ? (DateTime?)null : dr.GetDateTime(5),
                        Precio = dr.GetDecimal(6),
                        Stock = dr.GetInt32(7),
                        Imagen = dr.GetString(8)
                    });
                }
                dr.Close();
            }
            return productos;
        }
        public IEnumerable<Producto> obtenerProductosxCategoria(int idCategoria)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarProductosxCategoria", cnx);
                cmd.Parameters.AddWithValue("@idCat", idCategoria);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    productos.Add(new Producto
                    {
                        IdProducto = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Descripcion = dr.GetString(2),
                        NombreCategoria = dr.GetString(3),
                        FechaFabricacion = dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4),
                        FechaVencimiento = dr.IsDBNull(5) ? (DateTime?)null : dr.GetDateTime(5),
                        Precio = dr.GetDecimal(6),
                        Stock = dr.GetInt32(7),
                        Imagen = dr.GetString(8)
                    });
                }
                dr.Close();
            }
            return productos;
        }

        public Producto obtenerProductosxId(int id)
        {
            Producto? producto = null;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerProductoPorId", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    producto = new Producto
                    {
                        IdProducto = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Descripcion = dr.GetString(2),
                        idCategoria = dr.GetInt32(3),
                        NombreCategoria = dr.GetString(4),
                        FechaFabricacion = dr.IsDBNull(5) ? (DateTime?)null : dr.GetDateTime(5),
                        FechaVencimiento = dr.IsDBNull(6) ? (DateTime?)null : dr.GetDateTime(6),
                        Precio = dr.GetDecimal(7),
                        Stock = dr.GetInt32(8),
                        Imagen = dr.IsDBNull(9) ? null : dr.GetString(9)
                    };
                }
                dr.Close();
            }
            return producto;
        }
        public string EliminarProducto(int id)
        {
            string mensaje = "";

                using (SqlConnection cnx = new SqlConnection(_connection))
                {
                    try
                    {
                        cnx.Open();
                        SqlCommand cmd = new SqlCommand("sp_EliminarProducto", cnx);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdProducto", id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        mensaje = rowsAffected > 0 ? "Producto eliminado exitosamente." : "No se pudo eliminar el producto.";

                    }
                    catch (Exception ex)
                    {
                        mensaje = "Error al eliminar el producto: " + ex.Message;
                    }
                }
                return mensaje;
        }


       
    }
}
