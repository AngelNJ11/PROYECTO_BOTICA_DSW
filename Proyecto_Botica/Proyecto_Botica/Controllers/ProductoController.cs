
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Proyecto_Botica.Models;
using System.Data;
using System.Text;

namespace Proyecto_Botica.Controllers
{
    public class ProductoController : Controller
    {
       
        public async Task<IActionResult> ListarProducto()
        {
            List<Producto> lstProductos = new List<Producto>();
            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri("https://localhost:7191/api/Producto/");
                HttpResponseMessage response = await prod.GetAsync("obtenerProductos");
                string apiResponse = await response.Content.ReadAsStringAsync();
                lstProductos = JsonConvert.DeserializeObject<List<Producto>>(apiResponse).ToList();
            }
            return View(await Task.Run(() => lstProductos));
        }
        public async Task<IActionResult> EliminarProducto(int id)
        {
            string mensaje = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/api/Producto/");

                try
                {
                    HttpResponseMessage response = await client.DeleteAsync($"ElimEliminarProducto/{id}");

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
                    mensaje = "Error al intentar eliminar el producto: " + ex.Message;
                }
            }
            TempData["MensajeTipo"] = "eliminacion";
            TempData["Mensaje"] = "Producto Eliminado";

            return RedirectToAction("ListarProducto");
        }
        public async Task<IActionResult> AgregarProducto()
        {
            List<Categoria> temp = new List<Categoria>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/api/Categoria/"); 

                HttpResponseMessage responseMessage = await client.GetAsync("obtenerCategorias");

                if (responseMessage.IsSuccessStatusCode)
                {
                    string apiRest = await responseMessage.Content.ReadAsStringAsync();
                    temp = JsonConvert.DeserializeObject<List<Categoria>>(apiRest);
                }
                else
                {
                    ViewBag.Error = "No se pudo obtener las categorías. Intente nuevamente.";
                }
            }

            ViewBag.Categorias = new SelectList(temp, "IdCategoria", "Nombre");

            return View(new Producto());
        }
       
        [HttpPost]
        public async Task<IActionResult> AgregarProducto(Producto producto, IFormFile imagen)
        {
            string mensaje = "";

            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productos", imagen.FileName);
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }
                    producto.Imagen = "/img/productos/" + imagen.FileName;
                }
                else
                {
                    producto.Imagen = "/img/productos/default_sin_imagen.png";
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7191/api/Producto/");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync("GuardarProducto", content);
                    string apiResponse = await responseMessage.Content.ReadAsStringAsync();
                    mensaje = apiResponse.Trim();
                }

                TempData["Mensaje"] = "Producto Registrado";
                return RedirectToAction("ListarProducto");
            }

            return View(producto);
        }

        public async Task<IActionResult> EditProducto(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("ListarProducto");

            List<Categoria> categorias = new List<Categoria>();
            Producto producto = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/");

                HttpResponseMessage responseMessage = await client.GetAsync("api/Producto/obtenerProductoxId/" + id);
                string apiResponse = await responseMessage.Content.ReadAsStringAsync();
                producto = JsonConvert.DeserializeObject<Producto>(apiResponse);
                

                HttpResponseMessage responseCategorias = await client.GetAsync("api/Categoria/obtenerCategorias");
                string apiRestCategorias = await responseCategorias.Content.ReadAsStringAsync();
                categorias = JsonConvert.DeserializeObject<List<Categoria>>(apiRestCategorias).ToList();
                
            }
            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Nombre");
            return View(producto);
        }
        [HttpPost]
        public async Task<IActionResult> EditProducto(Producto producto, IFormFile? imagen)
        {
            string mensaje = "";

            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productos", imagen.FileName);
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }
                    producto.Imagen = "/img/productos/" + imagen.FileName;
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7191/");
                        HttpResponseMessage responseMessage = await client.GetAsync("api/Producto/obtenerProductoxId/" + producto.IdProducto);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            string apiResponse = await responseMessage.Content.ReadAsStringAsync();
                            Producto productoActual = JsonConvert.DeserializeObject<Producto>(apiResponse);
                            producto.Imagen = productoActual.Imagen;
                        }
                    }
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7191/api/Producto/");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PutAsync("ActualizarProducto", content);
                    string apiRes = await responseMessage.Content.ReadAsStringAsync();
                    mensaje = apiRes.Trim();
                }
                TempData["MensajeTipo"] = "actualizado";
                TempData["Mensaje"] = "Producto Actualizado";
                return RedirectToAction("ListarProducto");
            }
            return View(await Task.Run(() => producto));
        }


        public async Task<IActionResult> VerDetalleProducto(string id)
        {

            if (string.IsNullOrEmpty(id)) return RedirectToAction("ListarProducto");

            Producto producto = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/");

                HttpResponseMessage responseMessage = await client.GetAsync("api/Producto/obtenerProductoxId/" + id);
                string apiResponse = await responseMessage.Content.ReadAsStringAsync();
                producto = JsonConvert.DeserializeObject<Producto>(apiResponse);

            }
            return View(producto);
        }



    }
}
