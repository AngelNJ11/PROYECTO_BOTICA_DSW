using System.ComponentModel.DataAnnotations;

namespace Proyecto_Botica_API.Models
{
    public class Proveedor
    {
        public int idProveedor { get; set; }

        [Required(ErrorMessage = "El nombre del Proveedor es obligatorio.")]
        public string? nombre { get; set; }

        public int telefono { get; set; }

        [Required(ErrorMessage = "El Correo es obligatorio.")]
        public string? correo { get; set; }

        public string? direccion { get; set; }
    }
}
