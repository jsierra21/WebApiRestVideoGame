using Newtonsoft.Json;

namespace Core.DTOs
{
    public class VideoJuegosDto
    {
        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("compania")]
        public required string Compania { get; set; }

        [JsonProperty("anio_lanzamiento")]
        public required int AnioLanzamiento { get; set; }

        [JsonProperty("precio")]
        public required decimal Precio { get; set; }

        [JsonProperty("puntaje_promedio")]
        public decimal PuntajePromedio { get; set; } = 0m; // Valor por defecto

        [JsonProperty("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now; // Valor por defecto

        [JsonProperty("usuario")]
        public required string Usuario { get; set; }
    }
}
