using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Botica.Models;
using Proyecto_Botica.Repositorio;
using Proyecto_Botica.Repositorio.RepositorioSQL;

namespace Proyecto_Botica.Controllers
{
    public class VentaController : Controller
    {
        IProducto _producto;
        ICategoria _categoria;
        IVenta _venta;
        IDetalleVenta _detalleVenta;
        public VentaController ()
        {
            _producto = new productoSQL();
            _categoria = new categoriaSQL();
            _venta = new ventaSQL();
            _detalleVenta = new detVentaSQL();
        }

        public async Task<IActionResult> ListarProductosVenta (int idCategoria = 0)
        {
            List<Producto> lstProductos;
            var jsonData = HttpContext.Session.GetString("ItemList");
            var msjData = HttpContext.Session.GetString("ItemString");

            if (!string.IsNullOrEmpty(msjData))
            {
                var mensaje = System.Text.Json.JsonSerializer.Deserialize<string>(msjData);
                ViewBag.mensajeVenta = mensaje;
            }

            if (jsonData.IsNullOrEmpty())
            {
                lstProductos = new List<Producto>();
                HttpContext.Session.SetString("ItemList", System.Text.Json.JsonSerializer.Serialize(lstProductos));
            }else
            {
                lstProductos = System.Text.Json.JsonSerializer.Deserialize<List<Producto>>(jsonData);
            }
            decimal subtotal = lstProductos.Sum(x => x.Precio);

            ViewBag.idproducto = lstProductos.Count;
            ViewBag.subtotal = subtotal;
            ViewData["productosEscogidos"] = lstProductos;
            ViewData["Categorias"] = _categoria.obtenerCategorias();

            if (idCategoria == 0)
            {
                return View(await Task.Run(() => _producto.obtenerProductos()));
            }

            return View(await Task.Run(() => _producto.obtenerProductosxCategoria(idCategoria)));
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
            List<Producto> lstProductos;
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
            }
            return RedirectToAction("ListarProductosVenta");
        }
    }
}
