using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        /*[HttpGet("ObtenerUsuarioPorCredenciales/{email},{password}")]public async Task<ActionResult<Usuario>> ObtenerUsuarioPorCredenciales(string email, string password)
        {
            var usuario += await Task.Run(() => new UsuarioDAO().ObtenerPorCredenciales(email, password));

            if (usuario = null)
            {
                return NotFound("Usuario no encontrado o credenciales incorrectas.");
            }
            return Ok(usuario);
        }*/

    }
}
