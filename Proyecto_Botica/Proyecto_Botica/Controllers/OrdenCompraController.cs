    using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Botica.Models;
using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;
using System.Text;

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
            List<OrdenCompra> lstOrdenCompras = new List<OrdenCompra>();
            using(var orden = new HttpClient())
            {
                orden.BaseAddress = new Uri("https://localhost:7191/api/OrdenCompra/ListarOrdenCompra");
                HttpResponseMessage response = await orden.GetAsync("ListarOrdenCompra");
                string apiResponse = await response.Content.ReadAsStringAsync();
                lstOrdenCompras = JsonConvert.DeserializeObject<List<OrdenCompra>>(apiResponse).ToList();
            }

           return View(await Task.Run(() => lstOrdenCompras));
        }

        public async Task<IActionResult> Registrar()
        {
            return View(await Task.Run(() => new OrdenCompra()));
        }



        [HttpPost]
        public async Task<IActionResult> Registrar(OrdenCompra ordenCompra)
        {

            Console.WriteLine("Orden de compra : " + ordenCompra.IdProducto);


            string mensaje = "";

           
            
            using(var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:7191/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(ordenCompra), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync("api/OrdenCompra/registrarOrdenComprar", content);
                string apiResponse = await responseMessage.Content.ReadAsStringAsync();

                mensaje = apiResponse.Trim();

            }

            TempData["Mensaje"] = "Orden Compra registrado";

            return View(ordenCompra);



        }

    }
}
