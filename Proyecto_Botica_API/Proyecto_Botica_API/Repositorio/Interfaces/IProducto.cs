using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface IProducto
    {
        IEnumerable<Producto> obtenerProductos();
        IEnumerable<Producto> obtenerProductosxCategoria(int idCategoria);
        Producto obtenerProductosxId(int idProducto);
        string ActualizarProducto(Producto producto);
        string GuardarProducto(Producto producto);
        string EliminarProducto(int id);

    }
}
