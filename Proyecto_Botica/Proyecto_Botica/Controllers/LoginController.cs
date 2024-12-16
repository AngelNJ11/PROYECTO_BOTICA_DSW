using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
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
            Usuario usuario = await ObtenerUsuarioPorCredenciales(Email, Password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("UsuarioApellido", usuario.ApellidoPat);

                if (usuario.Rol == "Administrador")
                {
                    return RedirectToAction("Index", "Administrador");
                }
                else if (usuario.Rol == "Cajero")
                {
                    return RedirectToAction("ListarProductosVenta", "Venta");
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

        private async Task<Usuario> ObtenerUsuarioPorCredenciales(string email, string password)
        {
            Usuario usuario = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/api/Login/");
                string requestUrl = $"obtenerUsuarioPorCredenciales?correo={email}&contrasenia={password}";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(apiResponse);
                }
            }

            return usuario;
        }
    }
}
