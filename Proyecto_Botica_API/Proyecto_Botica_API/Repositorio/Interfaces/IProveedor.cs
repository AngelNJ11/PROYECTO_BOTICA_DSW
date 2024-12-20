using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface IProveedor
    {
        IEnumerable<Proveedor> ListaProveedores();
        Proveedor BuscarProveedorPorId(int id);
        string EliminarProveedor(int id);
        string GuardarProveedor(Proveedor proveedor);
        string ActualizarProveedor(Proveedor proveedor);
    }
}
