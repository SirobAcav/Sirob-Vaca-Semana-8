namespace TransaccionesEFCore.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public decimal Cuenta { get; set; }
    }
}
