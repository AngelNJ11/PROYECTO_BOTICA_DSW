using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;

namespace Proyecto_Botica.Repositorio.RepositorioSQL
{
    public class detVentaSQL 
    {
        private readonly string _connection;
        public detVentaSQL()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cnx");
        }
       /* public int registrarDetVenta(DetalleVenta detVenta)
        {
            int rowsAffected;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("usp_registrarDetalleVenta", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_venta", detVenta.IdVenta);
                cmd.Parameters.AddWithValue("@id_producto", detVenta.IdProducto);
                cmd.Parameters.AddWithValue("@cantidad", detVenta.cantidad);
                cmd.Parameters.AddWithValue("@precio", detVenta.precio);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected;
        }*/
    }
}
