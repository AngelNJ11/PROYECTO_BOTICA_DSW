using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Botica.Models;
using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Proyecto_Botica.Controllers
{
    public class VentaController : Controller
    {
        public async Task<IActionResult> ListarProductosVenta(int idCategoria = 0)
        {
            List<Producto> lstProductos;
            var jsonData = HttpContext.Session.GetString("ItemList");
            var msjData = HttpContext.Session.GetString("ItemString");

            if (!string.IsNullOrEmpty(msjData))
            {
                var mensaje = System.Text.Json.JsonSerializer.Deserialize<string>(msjData);
                ViewBag.mensajeVenta = mensaje;
                HttpContext.Session.SetString("ItemString", System.Text.Json.JsonSerializer.Serialize(""));
            }

            if (jsonData.IsNullOrEmpty())
            {
                lstProductos = new List<Producto>();
                HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
            }
            else
            {
                lstProductos = System.Text.Json.JsonSerializer.Deserialize<List<Producto>>(jsonData);
            }
            decimal subtotal = lstProductos.Sum(x => x.Precio);

            ViewBag.idproducto = lstProductos.Count;
            ViewBag.subtotal = subtotal;
            ViewData["productosEscogidos"] = lstProductos;

            List<Categoria> categorias = new List<Categoria>();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7191/api/Categoria/");
                HttpResponseMessage response = await httpClient.GetAsync("obtenerCategorias");

                if (response.IsSuccessStatusCode)
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    categorias = JsonConvert.DeserializeObject<List<Categoria>>(apiRes)?.ToList() ?? new List<Categoria>();
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al obtener las categorías.";
                }
            }
            ViewData["Categorias"] = categorias;

            List<Producto> productos = new List<Producto>();
            using (var httpClient = new HttpClient())
            {
                if (idCategoria == 0)
                {
                    httpClient.BaseAddress = new Uri("https://localhost:7191/api/Producto/");
                    HttpResponseMessage response = await httpClient.GetAsync("obtenerProductos");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        productos = JsonConvert.DeserializeObject<List<Producto>>(apiRes)?.ToList() ?? new List<Producto>();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al obtener los productos.";
                    }
                }
                else
                {
                    httpClient.BaseAddress = new Uri("https://localhost:7191/api/Producto/");
                    HttpResponseMessage response = await httpClient.GetAsync($"obtenerProductosxCategoria/"+idCategoria);

                    if (response.IsSuccessStatusCode)
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        productos = JsonConvert.DeserializeObject<List<Producto>>(apiRes)?.ToList() ?? new List<Producto>();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al obtener los productos por categoría.";
                    }
                }
            }

            return View(productos);
        }

        public async Task<IActionResult> Comprar()
        {
            List<Producto> lstProductos;
            var jsonData = HttpContext.Session.GetString("ItemList");
            lstProductos = System.Text.Json.JsonSerializer.Deserialize<List<Producto>>(jsonData);
            
            decimal subtotal = lstProductos.Sum(x => x.Precio);
            Venta venta = null;
            DetalleVenta detVenta = null;
            int registro = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/api/Venta/");
                StringContent contentVenta = new StringContent(JsonConvert.SerializeObject(subtotal), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessageVenta = await client.PostAsync("registrarVenta", contentVenta);
                
                HttpResponseMessage responseMessageObtenerVenta = await client.GetAsync("obtenerUltimoRegistroVenta");
                string apiResponseObtenerVenta = await responseMessageObtenerVenta.Content.ReadAsStringAsync();
                venta = JsonConvert.DeserializeObject<Venta>(apiResponseObtenerVenta);

                using (var client2 = new HttpClient())
                {
                    client2.BaseAddress = new Uri("https://localhost:7191/api/DetalleVenta/");
                    for (int i = 0; i < lstProductos.Count; i++)
                    {
                        detVenta = new DetalleVenta
                        {
                            IdVenta = venta.Id,
                            IdProducto = lstProductos[i].IdProducto,
                            cantidad = 1,
                            precio = lstProductos[i].Precio,
                        };

                        
                        StringContent content = new StringContent(JsonConvert.SerializeObject(detVenta), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseMessage = await client2.PostAsync("registrarDetVenta", content);
                        string apiResponse = await responseMessage.Content.ReadAsStringAsync();

                        registro = int.Parse(apiResponse);
                    }
                }
            }
            if (registro > 0)
            {
                HttpContext.Session.SetString("ItemString", System.Text.Json.JsonSerializer.Serialize("Venta exitosa"));
                lstProductos.Clear();
                HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
            }
            
            return RedirectToAction("ListarProductosVenta");
        }

        public IActionResult eliminarProductoDeCarrito(int idProducto)
        {
            List<Producto> lstProductos;
            var jsonData = HttpContext.Session.GetString("ItemList");
            lstProductos = System.Text.Json.JsonSerializer.Deserialize<List<Producto>>(jsonData);

            var productoExistente = lstProductos.FirstOrDefault(p => p.IdProducto == idProducto);
            if (productoExistente != null)
            {
                lstProductos.Remove(productoExistente);
            }

            HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
            return RedirectToAction("ListarProductosVenta");
        }

        public async Task<IActionResult> agregarProductoAlCarrito(int idProducto = 0)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7191/");
                HttpResponseMessage responseMessage = await client.GetAsync("api/Producto/obtenerProductoxId/" + idProducto);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string apiResponse = await responseMessage.Content.ReadAsStringAsync();
                    Producto productoActual = JsonConvert.DeserializeObject<Producto>(apiResponse);
                    List<Producto> lstProductos;
                    var jsonData = HttpContext.Session.GetString("ItemList");
                    //Producto productoNuevo = _producto.obtenerProductosxId(idProducto);

                    if (jsonData.IsNullOrEmpty())
                    {
                        lstProductos = new List<Producto>();
                        HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
                    }
                    else
                    {
                        lstProductos = System.Text.Json.JsonSerializer.Deserialize<List<Producto>>(jsonData);

                        if (idProducto != 0)
                        {
                            var productoExistente = lstProductos.FirstOrDefault(p => p.IdProducto == idProducto);
                            if (productoExistente == null)
                            {
                                lstProductos.Add(productoActual);
                                HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
                            }
                        }
                    }
                    
                }
                return RedirectToAction("ListarProductosVenta");
            }
            
        }
    }
}
