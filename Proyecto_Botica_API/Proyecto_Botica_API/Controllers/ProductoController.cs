using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        [HttpGet("obtenerProductoxId/{id}")]public async Task<ActionResult<Producto>> obtenerProductoxId(int id)
        {
            var producto = await Task.Run(() => new ProductoDAO().obtenerProductosxId(id));
            if (producto == null)
            {
                return NotFound("Producto no encontrado.");
            }
            return Ok(producto);
        }
        [HttpGet("obtenerProductos")]public async Task<ActionResult<List<Producto>>> obtenerProductos()
        {
            var list = await Task.Run(() => new ProductoDAO().obtenerProductos());
            return Ok(list);
        }
        [HttpGet("obtenerProductosxCategoria/{idCategoria}")]public async Task<ActionResult<List<Producto>>> obtenerProductosxCategoria(int idCategoria)
        {
            var list = await Task.Run(() => new ProductoDAO().obtenerProductosxCategoria(idCategoria));

            if (list == null || !list.Any())
            {
                return NotFound("Producto no encontrado para la categoría especificada.");
            }

            return Ok(list);
        }
        [HttpPost("GuardarProducto")] public async Task<ActionResult<string>>GuardarProducto(Producto producto)
        {
            var mensaje = await Task.Run(() => new ProductoDAO().GuardarProducto(producto));
            return Ok(mensaje);
        }
        [HttpPut("ActualizarProducto")] public async Task<ActionResult<string>> ActualizarProducto(Producto producto)
        {
            var mensaje = await Task.Run(() => new ProductoDAO().ActualizarProducto(producto));
            return Ok(mensaje);
        }
        [HttpDelete("ElimEliminarProducto/{id}")]public async Task<ActionResult<string>> EliminarProducto(int id)
        {
            var mensaje = await Task.Run(() => new ProductoDAO().EliminarProducto(id));
            return Ok(mensaje);
        }
    }
}
