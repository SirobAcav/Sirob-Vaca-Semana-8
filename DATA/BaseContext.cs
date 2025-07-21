using Microsoft.EntityFrameworkCore;
using TransaccionesEFCore.Models;

namespace TransaccionesEFCore.DATA
{
    public class BaseContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cadenaConexion = "server=localhost;database=semana8_clientesdb;user=root;";
            optionsBuilder.UseMySql(cadenaConexion, ServerVersion.AutoDetect(cadenaConexion));
        }
    }
}
