using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_Botica.Models;
using System.Data;

namespace Proyecto_Botica.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IConfiguration configuration;

        public ProveedorController(IConfiguration config)
        {
            configuration = config;
        }

        public IEnumerable<Proveedor> ListaProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarProveedores", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    proveedores.Add(new Proveedor
                    {
                        idProveedor = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        telefono = int.Parse(dr.GetString(2)),
                        correo = dr.GetString(3),
                        direccion = dr.IsDBNull(4) ? null : dr.GetString(4)
                    });
                }
                dr.Close();
            }
            return proveedores;
        }

        public async Task<IActionResult> ListarProveedor()
        {
            var proveedores = await Task.Run(() => ListaProveedores());
            return View(proveedores);
        }

        private Proveedor BuscarProveedorPorId(int id)
        {
            Proveedor proveedor = null;
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_BuscarProveedorPorID", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proveedor", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    proveedor = new Proveedor
                    {
                        idProveedor = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        telefono = int.Parse(dr.GetString(2)),
                        correo = dr.GetString(3),
                        direccion = dr.IsDBNull(4) ? null : dr.GetString(4)
                    };
                }
                dr.Close();
            }
            return proveedor;
        }

        private string GuardarProveedor(Proveedor proveedor)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd;
                    if (proveedor.idProveedor == 0)
                    {
                        cmd = new SqlCommand("sp_RegistrarProveedor", cnx);
                    }
                    else
                    {
                        cmd = new SqlCommand("sp_ActualizarProveedor", cnx);
                        cmd.Parameters.AddWithValue("@id_proveedor", proveedor.idProveedor);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", proveedor.nombre);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@correo", proveedor.correo);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.direccion ?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Proveedor registrado ." : "No se pudo registro el proveedor.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al registrar el proveedor: " + ex.Message;
                }
            }
            return mensaje;
        }

        public async Task<IActionResult> AgregarProveedor()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProveedor(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                var mensaje = await Task.Run(() => GuardarProveedor(proveedor));
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("ListarProveedor");
            }

            return View(proveedor);
        }

        public async Task<IActionResult> EditProveedor(int id)
        {
            var proveedor = await Task.Run(() => BuscarProveedorPorId(id));

            if (proveedor == null)
            {
                return RedirectToAction("ListarProveedor");
            }

            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> EditProveedor(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }

            var mensaje = await Task.Run(() => GuardarProveedor(proveedor));
            TempData["Mensaje"] = mensaje;

            return RedirectToAction("ListarProveedor");
        }

        public async Task<IActionResult> EliminarProveedor(int id)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(configuration["ConnectionStrings:cnx"]))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarProveedor", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_proveedor", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    mensaje = rowsAffected > 0 ? "Proveedor eliminado exitosamente." : "No se pudo eliminar el proveedor.";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al eliminar el proveedor: " + ex.Message;
                }
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("ListarProveedor");
        }
    }
}
