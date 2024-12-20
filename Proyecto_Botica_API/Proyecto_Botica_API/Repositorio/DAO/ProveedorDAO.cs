using Microsoft.Data.SqlClient;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.Interfaces;
using System.Data;

namespace Proyecto_Botica_API.Repositorio.DAO
{
    public class ProveedorDAO : IProveedor
    {
        private readonly string _connection;
        public ProveedorDAO()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cnx");
        }
        public Proveedor BuscarProveedorPorId(int id)
        {
            Proveedor? proveedor = null;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_BuscarProveedorPorID", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proveedor", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    proveedor = new Proveedor
                    {
                        idProveedor = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        telefono = int.Parse(dr.GetString(2)),
                        correo = dr.GetString(3),
                        direccion = dr.IsDBNull(4) ? null : dr.GetString(4)
                    };
                }
                dr.Close();
            }
            return proveedor;
        }
        public string EliminarProveedor(int id)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_EliminarProveedor", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proveedor", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                mensaje = rowsAffected > 0 ? "Proveedor eliminado exitosamente." : "No se pudo eliminar el proveedor.";
            }
            return mensaje;
        }
        public string GuardarProveedor(Proveedor proveedor)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProveedor", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", proveedor.nombre);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@correo", proveedor.correo);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.direccion ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Proveedor registrado ." : "No se pudo registro el proveedor.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al registrar el proveedor: " + ex.Message;
                }
            }
            return mensaje;
        }
        public string ActualizarProveedor(Proveedor proveedor)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("sp_ActualizarProveedor", cnx);
                    cmd.Parameters.AddWithValue("@id_proveedor", proveedor.idProveedor);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", proveedor.nombre);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@correo", proveedor.correo);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.direccion ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Proveedor Actualizado ." : "No se pudo Actualizar el proveedor.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al Actualizar el proveedor: " + ex.Message;
                }
            }
            return mensaje;
        }
        public IEnumerable<Proveedor> ListaProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarProveedores", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    proveedores.Add(new Proveedor
                    {
                        idProveedor = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        telefono = int.Parse(dr.GetString(2)),
                        correo = dr.GetString(3),
                        direccion = dr.IsDBNull(4) ? null : dr.GetString(4)
                    });
                }
                dr.Close();
            }
            return proveedores;
        }
        
    }
}
