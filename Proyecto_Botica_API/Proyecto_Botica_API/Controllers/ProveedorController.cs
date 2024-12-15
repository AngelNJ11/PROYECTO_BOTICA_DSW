using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        [HttpGet("ListaProveedores")]public async Task<ActionResult<List<Proveedor>>> ListaProveedores()
        {
            var list = await Task.Run(() => new ProveedorDAO().ListaProveedores());
            return Ok(list);
        }
        [HttpGet("BuscarProveedorPorId/{id}")]public async Task<ActionResult<List<Proveedor>>> BuscarProveedorPorId(int id)
        {
            var list = await Task.Run(() => new ProveedorDAO().BuscarProveedorPorId(id));
            return Ok(list);
        }
        [HttpPost("GuardarProveedor")]public async Task<ActionResult<string>> GuardarProveedor(Proveedor proveedor)
        {
            var mensaje = await Task.Run(() => new ProveedorDAO().GuardarProveedor(proveedor));
            return Ok(mensaje);
        }
        [HttpPut("ActualizarProveedor")] public async Task<ActionResult<string>> ActualizarProveedor(Proveedor proveedor)
        {
            var mensaje = await Task.Run(() => new ProveedorDAO().ActualizarProveedor(proveedor));
            return Ok(mensaje);
        }
        [HttpDelete("EliminarProveedor/{id}")]public async Task<ActionResult<string>> EliminarProveedor(int id)
        {
            var mensaje = await Task.Run(() => new ProveedorDAO().EliminarProveedor(id));
            return Ok(mensaje);
        }
    }
}
