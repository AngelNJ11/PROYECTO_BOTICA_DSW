using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        [HttpGet("obtenerUltimoRegistroVenta")]public async Task<ActionResult<List<Venta>>> obtenerUltimoRegistroVenta()
        {
            var list = await Task.Run(() => new VentaDAO().obtenerUltimoRegistroVenta());
            return Ok(list);
        }

        [HttpPost("registrarVenta")]public async Task<ActionResult<Venta>>RegistrarVenta(decimal precio)
        {
            var mensaje = await Task.Run(() => new VentaDAO().registrarVenta(precio));
            return Ok(mensaje);
        }
    }
}
