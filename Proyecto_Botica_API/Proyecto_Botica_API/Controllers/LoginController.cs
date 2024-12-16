using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;
using Proyecto_Botica_API.Repositorio.Interfaces;
using System.Data;

namespace Proyecto_Botica_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("obtenerUsuarioPorCredenciales")]
        public async Task<IActionResult> ObtenerUsuarioPorCredenciales(string correo, string contrasenia)
        {
            Usuario usuario = null;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnx")))
            {
                SqlCommand command = new SqlCommand("sp_UsuarioLoginn", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Correo", correo);
                command.Parameters.AddWithValue("@Contrasenia", contrasenia);

                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();

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

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(usuario);
        }
    }
}
