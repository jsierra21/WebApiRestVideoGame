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
        public DbSet<VideojuegosCalificaciones> VideojuegosCalificaciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración adicional para las entidades si es necesario
            modelBuilder.Entity<UsuarioEntity>().ToTable("Usuario"); // Cambia "Usuarios" por el nombre real de la tabla
            modelBuilder.Entity<VideoJuegosEntity>().ToTable("Videojuegos");
            modelBuilder.Entity<VideojuegosCalificaciones>().ToTable("Vw_VideojuegosCalificaciones");

            // Configure the relationship
            modelBuilder.Entity<VideojuegosCalificaciones>()
               .ToView("vw_VideojuegosCalificaciones") // Mapea la clase a la vista
               .HasKey(v => v.VideojuegoID); // Define la clave principal
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
