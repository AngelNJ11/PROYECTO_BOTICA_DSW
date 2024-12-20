using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface ICategoria
    {
        IEnumerable<Categoria> obtenerCategorias();
    }
}
