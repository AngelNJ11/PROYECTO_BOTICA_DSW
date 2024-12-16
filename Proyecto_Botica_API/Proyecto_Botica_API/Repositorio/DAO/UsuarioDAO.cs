using Microsoft.AspNetCore.Mvc;
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

        public async Task<Usuario> ObtenerUsuarioPorCredenciales(string email, string password)
        {
            Usuario? usuario = null;

            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                await cnx.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_UsuarioLoginn", cnx)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Correo", email);
                cmd.Parameters.AddWithValue("@Contrasenia", password);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
    }
}
