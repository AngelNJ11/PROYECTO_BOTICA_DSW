using Proyecto_Botica.Models;

namespace Proyecto_Botica.Repositorio
{
    public interface IVenta
    {
        void registrarVenta(decimal precio);
        Venta obtenerUltimoRegistroVenta();
    }
}
