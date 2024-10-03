using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Videojuegos")]
public class VideoJuegosEntity
{
    [Key] // Clave primaria
    [Column("VideojuegoID")]
    public int VideojuegoID { get; set; }

    [Required] // Hace que esta propiedad sea obligatoria
    [Column("Nombre")]
    [MaxLength(255)] // Limita la longitud máxima de la cadena a 255
    public string Nombre { get; set; }

    [Required]
    [Column("Compania")]
    [MaxLength(255)]
    public string Compania { get; set; }

    [Required]
    [Column("AnioLanzamiento")]
    public int AnioLanzamiento { get; set; }

    [Required]
    [Column("Precio")]
    public decimal Precio { get; set; } // Decimales 10, 2

    [Required]
    [Column("PuntajePromedio")]
    public decimal PuntajePromedio { get; set; } = 0m; // Valor por defecto es 0

    [Required]
    [Column("FechaActualizacion")]
    public DateTime FechaActualizacion { get; set; } = DateTime.Now; // Valor por defecto es la fecha actual

    [Required]
    [Column("Usuario")]
    [MaxLength(50)]
    public string Usuario { get; set; }
}
