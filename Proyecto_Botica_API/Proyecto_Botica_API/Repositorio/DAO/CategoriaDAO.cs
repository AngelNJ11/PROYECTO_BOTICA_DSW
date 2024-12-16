﻿using Microsoft.Data.SqlClient;
using Proyecto_Botica_API.Models;
using Proyecto_Botica_API.Repositorio.Interfaces;

namespace Proyecto_Botica_API.Repositorio.DAO
{
    public class CategoriaDAO : ICategoria
    {
        private readonly string _connection;
        public CategoriaDAO()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cnx");
        }
        public IEnumerable<Categoria> obtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarCategorias", cnx);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    categorias.Add(new Categoria
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return categorias;
        }
    }
}