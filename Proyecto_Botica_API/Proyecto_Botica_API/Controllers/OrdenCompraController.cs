using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica.Repositorio.RepositorioSQL;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenCompraController : ControllerBase
    {
        [HttpGet("ListarOrdenCompra")]
        public async Task<ActionResult<List<OrdenCompra>>> ListarOrdenCompra()
        {
            var list = await Task.Run(() => new OrdenCompraDAO().ListarOrdenCompra());
            return Ok(list);
        }

        [HttpPost("registrarOrdenComprar")]
        public async Task<ActionResult<string>> registrarOrdenComprar(OrdenCompra ordenCompra)
        {
            Console.WriteLine("Orden" + ordenCompra.IdProducto);


            var mensaje = await Task.Run(() => new OrdenCompraDAO().registrarOrdenComprar(ordenCompra));

            return Ok(mensaje);
        }

    }
}
