﻿using Proyecto_Botica_API.Repositorio.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using Proyecto_Botica_API.Models;

namespace Proyecto_Botica_API.Repositorio.DAO
{
    public class VentaDAO : IVenta
    {
        private readonly string _connection;
        public VentaDAO()
        {
            _connection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build().
                GetConnectionString("cnx");
        }

        public Venta obtenerUltimoRegistroVenta()
        {
            Venta? venta = null;
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("usp_obtenerUltimoRegistroVenta", cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    venta = new Venta
                    {
                        Id = dr.GetInt32(0),
                        Fecha = dr.GetDateTime(1),
                        total = dr.GetDecimal(2)
                    };
                }
                dr.Close();
            }
            return venta;
        }

        public string registrarVenta(decimal precio)
        {
            string mensaje = "";
            using (SqlConnection cnx = new SqlConnection(_connection))
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("usp_registrarVenta", cnx);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@total", precio);

                    cmd.ExecuteNonQuery();
                    mensaje = "${Se ha guardado una venta}";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al guardar la venta: " + ex.Message;
                }
            }
            return mensaje;
        }
    }
}
