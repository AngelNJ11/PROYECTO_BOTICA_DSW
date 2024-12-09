using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica.Models;
using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;

namespace Proyecto_Botica.Controllers
{
    public class OrdenCompraController : Controller
    {
        IOrdenCompra _ordenCompra;

        public OrdenCompraController()
        {
            _ordenCompra = new ordenCompraSQL();
        }

        public async Task<IActionResult> ListarOrdenCompra()
        {
           return View(await Task.Run(() => _ordenCompra.ListarOrdenCompra()));
        }

        public async Task<IActionResult> Registrar()
        {
            return View(await Task.Run(() => new OrdenCompra()));
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(OrdenCompra ordenCompra)
        {
            if (ModelState.IsValid)
            {
                var mensaje = await Task.Run(() => _ordenCompra.registrarOrdenComprar(ordenCompra));
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("ListarOrdenCompra");
            }

            return View(ordenCompra);
        }


    }
}
