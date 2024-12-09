using System.ComponentModel.DataAnnotations;

namespace Proyecto_Botica.Models
{
    public class OrdenCompra
    {

        public int IdOrdenCompra { get; set; }

        public int? IdProducto { get; set; }

        public int? IdProveedor { get; set; }

        public int? Cantidad { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }

        public decimal Precio { get; set; }

    }
}
