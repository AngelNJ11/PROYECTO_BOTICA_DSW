namespace Proyecto_Botica.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPat { get; set; }
        public string ApellidoMat { get; set; }
        public DateTime FechaNac { get; set; }
        public string DNI { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public decimal? Salario { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }
    }
}
