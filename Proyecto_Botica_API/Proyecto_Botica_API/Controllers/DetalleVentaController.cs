using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentaController : ControllerBase
    {

        [HttpPost("registrarDetVenta")]public async Task<ActionResult<int>>registrarDetVenta(DetalleVenta detVenta)
        {
            int mensaje = await Task.Run(() => new DetalleVentaDAO().registrarDetVenta(detVenta));
            return Ok(mensaje);
        }
    }
}
