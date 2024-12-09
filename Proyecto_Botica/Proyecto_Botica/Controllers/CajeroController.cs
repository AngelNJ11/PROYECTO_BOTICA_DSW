using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Botica.Controllers
{
    public class CajeroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
