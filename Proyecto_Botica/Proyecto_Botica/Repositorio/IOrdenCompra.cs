using Proyecto_Botica.Models;

namespace Proyecto_Botica.Repositorio
{
    public interface IOrdenCompra
    {

        public IEnumerable<OrdenCompra> ListarOrdenCompra();

        public String registrarOrdenComprar(OrdenCompra ordenCompra);


    }
}
