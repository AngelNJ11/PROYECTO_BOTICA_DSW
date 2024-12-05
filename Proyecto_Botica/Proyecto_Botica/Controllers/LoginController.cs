using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;

namespace Proyecto_Botica.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            string connectionString = _configuration.GetConnectionString("cnx");
            Usuario usuario = await Task.Run(() => ObtenerUsuarioPorCredenciales(Email, Password));

            if (usuario != null)
            {
                if (usuario.Rol == "Administrador")
                {
                    TempData["UsuarioNombre"] = usuario.Nombre;
                    TempData["UsuarioApellido"] = usuario.ApellidoPat;
                    return RedirectToAction("Index", "Administrador");
                }
                else if (usuario.Rol == "Cajero")
                {
                    return RedirectToAction("Index", "Cajero");
                }
                else
                {
                    ViewBag.Error = "Usuario no tiene un rol asignado.";
                }
            }
            else
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
            }

            return View("Index");
        }

        private Usuario ObtenerUsuarioPorCredenciales(string email, string password)
        {
            Usuario usuario = null;
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnx")))
            {
                SqlCommand command = new SqlCommand("sp_UsuarioLoginn", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Correo", email);
                command.Parameters.AddWithValue("@Contrasenia", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

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
