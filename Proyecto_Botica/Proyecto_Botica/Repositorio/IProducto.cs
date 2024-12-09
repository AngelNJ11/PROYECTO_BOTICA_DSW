using Proyecto_Botica.Models;

namespace Proyecto_Botica.Repositorio
{
    public interface IProducto
    {
        IEnumerable<Producto> obtenerProductos();
        IEnumerable<Producto> obtenerProductosxCategoria(int idCategoria);
        Producto obtenerProductosxId(int idProducto);
    }
}
