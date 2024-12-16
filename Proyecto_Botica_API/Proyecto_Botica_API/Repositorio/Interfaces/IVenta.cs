﻿using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.Interfaces
{
    public interface IVenta
    {
        void registrarVenta(decimal precio);
        Venta obtenerUltimoRegistroVenta();
    }
}