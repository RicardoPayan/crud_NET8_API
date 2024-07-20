namespace backend_examen.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Nombre{ get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public int Tipo { get; set; }
        public string Estado { get; set; }
        public string Ciudad { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
