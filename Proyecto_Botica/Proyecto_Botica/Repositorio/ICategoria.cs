using Proyecto_Botica.Models;

namespace Proyecto_Botica.Repositorio
{
    public interface ICategoria
    {
        IEnumerable<Categoria> obtenerCategorias();
    }
}
