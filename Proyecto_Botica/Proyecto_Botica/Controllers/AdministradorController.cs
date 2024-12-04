using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Botica.Controllers
{
    public class AdministradorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
