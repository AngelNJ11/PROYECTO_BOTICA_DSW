using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;

namespace Proyecto_Botica.Repositorio.RepositorioSQL
{
    public class ordenCompraSQL : IOrdenCompra
    {

        private readonly string _connection;
        public ordenCompraSQL()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cnx");
        }

        public IEnumerable<OrdenCompra> ListarOrdenCompra()
        {

            List<OrdenCompra> ordenCompras = new List<OrdenCompra>();

            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarOrdenComprar", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ordenCompras.Add(new OrdenCompra
                    { 
                        IdOrdenCompra = dr.GetInt32(0),
                        IdProducto = dr.GetInt32(1),
                        IdProveedor = dr.GetInt32(2),
                        Cantidad = dr.GetInt32(3),
                        Fecha = dr.IsDBNull(4) ? (DateTime?) null : dr.GetDateTime(4),
                        Precio = dr.GetDecimal(5)
                    });
                }
                dr.Close();
            }
            return ordenCompras;
        }

        public String registrarOrdenComprar(OrdenCompra ordenCompra)
        {

            String mensaje = ""; 

            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();

                SqlCommand cmd = new SqlCommand("sp_RegistrarOrdenCompra", cnx);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdProducto", ordenCompra.IdProducto);
                cmd.Parameters.AddWithValue("@IdProveedor", ordenCompra.IdProveedor);
                cmd.Parameters.AddWithValue("@Cantidad", ordenCompra.Cantidad);
                cmd.Parameters.AddWithValue("@Fecha", ordenCompra.Fecha);
                cmd.Parameters.AddWithValue("@Precio", ordenCompra.Precio);

                cmd.ExecuteNonQuery();
                mensaje = "¡Éxito! La orden de compra fue registrada correctamente.";

            }

            return mensaje;
        }
    }
}
