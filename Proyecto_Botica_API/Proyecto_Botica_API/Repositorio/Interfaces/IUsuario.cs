using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface IUsuario
    {
        Usuario ObtenerUsuarioPorCredenciales(string email, string password);
    }
}
