
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Proyecto_Botica.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration configuration;

        public ProductoController(IConfiguration config)
        {
            configuration = config;
        }
        public IEnumerable<Producto> ListaProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarProductos", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    productos.Add(new Producto
                    {
                        IdProducto = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Descripcion = dr.GetString(2),
                        NombreCategoria = dr.GetString(3),
                        FechaFabricacion = dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4),
                        FechaVencimiento = dr.IsDBNull(5) ? (DateTime?)null : dr.GetDateTime(5),
                        Precio = dr.GetDecimal(6),
                        Stock = dr.GetInt32(7),
                        Imagen = dr.GetString(8)
                    });
                }
                dr.Close();
            }
            return productos;
        }
        public async Task<IActionResult> ListarProducto()
        {
            var productos = await Task.Run(() => ListaProductos());
            var categorias = ListaCategorias().ToDictionary(c => c.IdCategoria, c => c.Nombre);
            ViewBag.Categorias = categorias;

            return View(productos);
        }
        public IEnumerable<Categoria> ListaCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarCategorias", cnx);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    categorias.Add(new Categoria
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return categorias;
        }
        private Producto BuscarProductoPorId(int id)
        {
            Producto producto = null;
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerProductoPorId", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    producto = new Producto
                    {
                        IdProducto = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Descripcion = dr.GetString(2),
                        idCategoria = dr.GetInt32(3),
                        FechaFabricacion = dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4),
                        FechaVencimiento = dr.IsDBNull(5) ? (DateTime?)null : dr.GetDateTime(5),
                        Precio = dr.GetDecimal(6),
                        Stock = dr.GetInt32(7),
                        Imagen = dr.GetString(8)
                    };
                }
                dr.Close();
            }
            return producto;
        }
        private string GuardarProducto(Producto producto)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarProducto", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto == 0 ? (object)DBNull.Value : producto.IdProducto);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.idCategoria);
                    cmd.Parameters.AddWithValue("@FechaFabricacion", producto.FechaFabricacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaVencimiento", producto.FechaVencimiento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@Imagen", producto.Imagen);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Producto guardado exitosamente." : "No se pudo guardar el producto.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al guardar el producto: " + ex.Message;
                }
            }
            return mensaje;
        }

        public async Task<IActionResult> AgregarProducto()
        {
            var categorias = await Task.Run(() => ListaCategorias());
            ViewBag.Categorias = categorias;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProducto(Producto producto, IFormFile imagen)
        {


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

                var mensaje = await Task.Run(() => GuardarProducto(producto));
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("ListarProducto");
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }

            var categorias = await Task.Run(() => ListaCategorias());
            ViewBag.Categorias = categorias;

            return View(producto);
        }




        public async Task<IActionResult> EditProducto(int id)
        {
            var producto = await Task.Run(() => BuscarProductoPorId(id));

            if (producto == null)
            {
                return RedirectToAction("ListarProducto");
            }
            ViewBag.Categorias = new SelectList(await Task.Run(() => ListaCategorias()), "IdCategoria", "Nombre", producto.idCategoria);

            return View(producto);
        }


        [HttpPost]
        public async Task<IActionResult> EditProducto(Producto producto, IFormFile imagen)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(await Task.Run(() => ListaCategorias()), "IdCategoria", "Nombre", producto.idCategoria);
                return View(producto);
            }

            if (imagen != null && imagen.Length > 0)
            {
                var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productos", imagen.FileName);
                using (var stream = new FileStream(rutaImagen, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }
                producto.Imagen = "/img/productos/" + imagen.FileName;
            }

            var mensaje = await Task.Run(() => GuardarProducto(producto));
            TempData["Mensaje"] = mensaje;

            return RedirectToAction("ListarProducto");
        }



        public async Task<IActionResult> EliminarProducto(int id)
        {
            string mensaje = "";
            string rutaImagen = "";


            var producto = await Task.Run(() => BuscarProductoPorId(id));
            if (producto != null)
            {
                rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", producto.Imagen.TrimStart('/'));


                using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
                {
                    try
                    {
                        cnx.Open();
                        SqlCommand cmd = new SqlCommand("sp_EliminarProducto", cnx);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdProducto", id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        mensaje = rowsAffected > 0 ? "Producto eliminado exitosamente." : "No se pudo eliminar el producto.";


                        if (rowsAffected > 0 && System.IO.File.Exists(rutaImagen))
                        {
                            System.IO.File.Delete(rutaImagen);
                        }
                    }
                    catch (Exception ex)
                    {
                        mensaje = "Error al eliminar el producto: " + ex.Message;
                    }
                }
            }
            else
            {
                mensaje = "El producto no existe.";
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("ListarProducto");
        }
    }
}
