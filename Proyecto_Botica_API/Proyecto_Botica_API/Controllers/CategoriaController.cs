using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.DAO;

namespace Proyecto_Botica_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        [HttpGet("obtenerCategorias")]
        public async Task<ActionResult<List<Categoria>>> obtenerCategorias()
        {
            var list = await Task.Run(() => new CategoriaDAO().obtenerCategorias());
            return Ok(list);
        }
    }
}
