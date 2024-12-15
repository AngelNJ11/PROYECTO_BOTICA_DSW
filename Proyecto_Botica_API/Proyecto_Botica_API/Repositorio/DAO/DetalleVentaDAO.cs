using Microsoft.Data.SqlClient;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.Interfaces;
using System.Data;

namespace Proyecto_Botica_API.Repositorio.DAO
{
    public class DetalleVentaDAO : IDetalleVenta
    {
        private readonly string _connection;
        public DetalleVentaDAO()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cnx");
        }
        public int registrarDetVenta(DetalleVenta detVenta)
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
        }
    }
}
