using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.Interfaces;
using System.Data;

namespace Proyecto_Botica_API.Repositorio.DAO
{
    public class UsuarioDAO: IUsuario
    {
        private readonly string _connection;
        public UsuarioDAO()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cnx");
        }

        public Usuario ObtenerUsuarioPorCredenciales(string email, string password)
        {
            Usuario? usuario = null;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_UsuarioLoginn", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", email);
                cmd.Parameters.AddWithValue("@Contrasenia", password);
                cnx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = (int)reader["id_usuario"],
                        Nombre = reader["nombre"].ToString(),
                        ApellidoPat = reader["apellido_pat"].ToString(),
                        ApellidoMat = reader["apellido_mat"].ToString(),
                        Correo = reader["correo"].ToString(),
                        Rol = reader["Rol"]?.ToString()
                    };
                }
                reader.Close();
            }
            return usuario;
        }

            return usuario;
        {
            throw new NotImplementedException();
        }
    }
}
