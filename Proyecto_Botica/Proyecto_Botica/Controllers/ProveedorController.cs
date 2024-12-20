using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Proyecto_Botica.Models;
using System.Data;
using System.Text;

namespace Proyecto_Botica.Controllers
{
    public class ProveedorController : Controller
    {
        public async Task<IActionResult> ListarProveedor()
        {
            List<Proveedor> temporal = new List<Proveedor>();
            using(var provee =  new HttpClient())
            {
                provee.BaseAddress = new Uri("https://localhost:7191/api/Proveedor/");
                HttpResponseMessage response = await provee.GetAsync("ListaProveedores");
                string apiRes = await response.Content.ReadAsStringAsync();
                temporal=JsonConvert.DeserializeObject<List<Proveedor>>(apiRes).ToList();

                    
            }
            return View(await Task.Run(() => temporal));
        }

        public async Task<IActionResult> AgregarProveedor()
        {
            return View(await Task.Run(()=> new Proveedor()));
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProveedor(Proveedor proveedor)
        {
            string mensaje = "";
            using (var provee = new HttpClient())
            {
                provee.BaseAddress = new Uri("https://localhost:7191/api/Proveedor/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(proveedor),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await provee.PostAsync("GuardarProveedor", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse.Trim();
            }

            TempData["Mensaje"] = "Proveedor Registrado";

            return RedirectToAction("ListarProveedor");
        }

        public async Task<IActionResult> EditProveedor(int id)
        {
            if (id <= 0)
                return RedirectToAction("ListarProveedor");

            Proveedor reg = new Proveedor();
            using (var provee = new HttpClient())
            {
                provee.BaseAddress = new Uri("https://localhost:7191/api/Proveedor/");
                HttpResponseMessage response = await provee.GetAsync("BuscarProveedorPorId/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reg = JsonConvert.DeserializeObject<Proveedor>(apiResponse);
            }
            return View(await Task.Run(() => reg));
        }


        [HttpPost]
        public async Task<IActionResult> EditProveedor(Proveedor proveedor)
        {
            string mensaje = "";
            using (var provee = new HttpClient())
            {
                provee.BaseAddress = new Uri("https://localhost:7191/api/Proveedor/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(proveedor),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await provee.PutAsync("ActualizarProveedor", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse;
            }
            TempData["MensajeTipo"] = "actualizado";
            TempData["Mensaje"] = "Proveedor Actualizado";
            return RedirectToAction("ListarProveedor");
        }

        public async Task<IActionResult> EliminarProveedor(int id)
        {
            string mensaje = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/api/Proveedor/");

                try
                {
                    HttpResponseMessage response = await client.DeleteAsync($"EliminarProveedor/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        mensaje = "Error al eliminar el proveedor. Intente nuevamente.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error al intentar eliminar el proveedor: " + ex.Message;
                }
            }
            TempData["MensajeTipo"] = "eliminacion";
            TempData["Mensaje"] = "Proveedor Eliminado";

            return RedirectToAction("ListarProveedor");
        }
    }
}
