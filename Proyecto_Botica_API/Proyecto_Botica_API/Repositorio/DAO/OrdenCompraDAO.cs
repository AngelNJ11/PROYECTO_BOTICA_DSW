using Microsoft.Data.SqlClient;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.Interfaces;
using System.Data;

namespace Proyecto_Botica.Repositorio.RepositorioSQL
{
    public class OrdenCompraDAO : IOrdenCompra
    {

        private readonly string _connection;
        public OrdenCompraDAO()
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
                        Fecha = dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4),
                        Precio = dr.GetDecimal(5)
                    });
                }
                dr.Close();
            }
            return ordenCompras;
        }

        public string registrarOrdenComprar(OrdenCompra ordenCompra)
        {

            string mensaje = "";

            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();

                SqlCommand cmd = new SqlCommand("sp_RegistrarOrdenCompra", cnx);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idProducto", ordenCompra.IdProducto);
                cmd.Parameters.AddWithValue("@idProveedor", ordenCompra.IdProveedor);
                cmd.Parameters.AddWithValue("@cantidad", ordenCompra.Cantidad);
                cmd.Parameters.AddWithValue("@fecha", ordenCompra.Fecha);
                cmd.Parameters.AddWithValue("@precio", ordenCompra.Precio);

                int rowsAffected = cmd.ExecuteNonQuery();
                mensaje = "¡Éxito! La orden de compra fue registrada correctamente.";

            }

            return mensaje;
        }

      
    }
}
