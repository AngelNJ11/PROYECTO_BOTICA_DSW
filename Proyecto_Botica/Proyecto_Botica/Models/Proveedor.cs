using System.ComponentModel.DataAnnotations;

namespace Proyecto_Botica.Models
{
    public class Proveedor
    {
        [Display(Name = "ID PROVEEDOR")] public int idProveedor { get; set; }

        [Required(ErrorMessage = "El nombre del Proveedor es obligatorio.")]
        [Display(Name = "NOMBRE")] public string? nombre { get; set; }

        [Display(Name = "TELEFONO")] public int telefono { get; set; }

        [Required(ErrorMessage = "El Correo es obligatorio.")]
        [Display(Name = "CORREO")] public string? correo { get; set; }

        [Display(Name = "DIRECCION")] public string? direccion { get; set; }
    }
}
