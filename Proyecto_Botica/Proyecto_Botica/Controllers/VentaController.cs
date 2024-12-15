using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Botica.Models;
using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Proyecto_Botica.Controllers
{
    public class VentaController : Controller
    {
        IVenta _venta;
        IDetalleVenta _detalleVenta;
        public VentaController ()
        {
            _venta = new ventaSQL();
            _detalleVenta = new detVentaSQL();
        }

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




        public IActionResult Comprar()
        {
            List<Producto> lstProductos;
            var jsonData = HttpContext.Session.GetString("ItemList");
            lstProductos = System.Text.Json.JsonSerializer.Deserialize<List<Producto>>(jsonData);
            
            decimal subtotal = lstProductos.Sum(x => x.Precio);
            _venta.registrarVenta(subtotal);
            Venta venta = _venta.obtenerUltimoRegistroVenta();
            DetalleVenta detVenta = null;
            int registro = 0;
            for (int i = 0; i < lstProductos.Count; i++)
            {
                detVenta = new DetalleVenta
                {
                    IdVenta = venta.Id,
                    IdProducto = lstProductos[i].IdProducto,
                    cantidad = 1,
                    precio = lstProductos[i].Precio,
                };
                registro = _detalleVenta.registrarDetVenta(detVenta);
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

        public IActionResult agregarProductoAlCarrito(int idProducto = 0)
        {
            /*List<Producto> lstProductos;
            var jsonData = HttpContext.Session.GetString("ItemList");
            Producto productoNuevo = _producto.obtenerProductosxId(idProducto);

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
                        lstProductos.Add(productoNuevo);
                        HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
                    }
                }
            }*/
            return RedirectToAction("ListarProductosVenta");
        }
    }
}
