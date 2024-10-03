using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Vw_VideojuegosCalificaciones")]
    public class VideojuegosCalificaciones
    {
        public int VideojuegoID { get; set; }          // ID del videojuego
        public string Nombre { get; set; }              // Nombre del videojuego
        public string Compania { get; set; }            // Compañía desarrolladora
        public int AnioLanzamiento { get; set; }        // Año de lanzamiento
        public decimal? Precio { get; set; }            // Precio del videojuego (nullable)
        public decimal? PuntuacionPromedio { get; set; } // Puntuación promedio de las calificaciones (nullable)
    }
}
