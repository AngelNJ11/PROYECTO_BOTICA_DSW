using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface IOrdenCompra
    {

        public IEnumerable<OrdenCompra> ListarOrdenCompra();

        public String registrarOrdenComprar(OrdenCompra ordenCompra);
    }
}
