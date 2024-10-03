using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DbModelContext : DbContext
    {
        public DbModelContext(DbContextOptions<DbModelContext> options) : base(options)
        {
        }

        // Propiedad DbSet para la entidad UsuarioEntity
        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<VideoJuegosEntity> VideoJuegos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración adicional para las entidades si es necesario
            modelBuilder.Entity<UsuarioEntity>().ToTable("Usuario"); // Cambia "Usuarios" por el nombre real de la tabla
            modelBuilder.Entity<VideoJuegosEntity>().ToTable("Videojuegos");
        }

        // (Opcional) Si necesitas configurar manualmente la conexión a la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("YourConnectionStringHere"); // Define tu cadena de conexión
            }
        }
    }
}
