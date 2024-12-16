using Microsoft.AspNetCore.Mvc;
using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface IUsuario
    {
        Task<Usuario> ObtenerUsuarioPorCredenciales(string email, string password);
    }
}
