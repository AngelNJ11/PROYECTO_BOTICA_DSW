using System.ComponentModel.DataAnnotations;
namespace Proyecto_Botica.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        public string? Descripcion { get; set; }

        public string? NombreCategoria { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaFabricacion { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int? idCategoria { get; set; }

        public string? Imagen { get; set; } 
    }
}
