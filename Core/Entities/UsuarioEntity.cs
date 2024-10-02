using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Usuario")]
    public class UsuarioEntity
    {
        [Key] // Indica que esta propiedad es la clave primaria
        [Column("Usr_IdUsuario")]
        public int IdUsuario { get; set; }

        [Required] // Hace que esta propiedad sea obligatoria
        [Column("Usr_nombre_usuario")]
        [MaxLength(100)] // Limita la longitud máxima de la cadena
        public string NombreUsuario { get; set; }

        [Required]
        [Column("Usr_correo_electronico")]
        [MaxLength(256)]
        public string CorreoElectronico { get; set; }

        [Required]
        [Column("Usr_Password")]
        [MaxLength(150)]
        public string Password { get; set; }

        [Required]
        [Column("FechaRegistro")]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [Column("CodUserUpdate")]
        [MaxLength(30)]
        public string CodUserUpdate { get; set; }

        [Required]
        [Column("FechaRegistroUpdate")]
        public DateTime FechaRegistroUpdate { get; set; }
    }
}
