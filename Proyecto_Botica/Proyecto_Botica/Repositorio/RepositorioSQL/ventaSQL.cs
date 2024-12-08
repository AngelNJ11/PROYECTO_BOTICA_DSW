using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;

namespace Proyecto_Botica.Repositorio.RepositorioSQL
{
    public class ventaSQL : IVenta
    {
        private readonly string _connection;
        public ventaSQL()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cnx");
        }

        public Venta obtenerUltimoRegistroVenta()
        {
            Venta venta = null;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("usp_obtenerUltimoRegistroVenta", cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    venta = new Venta
                    {
                        Fecha = dr.GetDateTime(0),
                        total = dr.GetDecimal(1),
                        Id = dr.GetInt32(2)
                    };
                }
                dr.Close();
            }
            return venta;
        }

        public void registrarVenta(decimal precio)
        {
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("usp_registrarVenta", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@total", precio);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
