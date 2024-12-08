using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;

namespace Proyecto_Botica.Repositorio.RepositorioSQL
{
    public class productoSQL : IProducto
    {
        private readonly string _connection;
        public productoSQL()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cnx");
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

        public Producto obtenerProductosxId(int idProducto)
        {
            Producto producto = null;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerProductoPorId", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);

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
                        Imagen = dr.GetString(9)
                    };
                }
                dr.Close();
            }
            return producto;
        }
    }
}
