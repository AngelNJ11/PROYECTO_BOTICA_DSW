namespace Proyecto_Botica.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
    }
}
