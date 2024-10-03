using Newtonsoft.Json;

namespace Core.DTOs
{
    public class VideoJuegosActualizarDto
    {
        [JsonProperty("video_juego_id")]
        public int video_juego_id { get; set; }

        [JsonProperty("nombre")]
        public required string nombre { get; set; }

        [JsonProperty("compania")]
        public required string compania { get; set; }

        [JsonProperty("anio_lanzamiento")]
        public required int anio_lanzamiento { get; set; }

        [JsonProperty("precio")]
        public required decimal precio { get; set; }

        [JsonProperty("puntaje_promedio")]
        public decimal puntaje_promedio { get; set; } = 0m; // Valor por defecto

        [JsonProperty("fecha_actualizacion")]
        public DateTime fecha_actualizacion { get; set; } = DateTime.Now; // Valor por defecto

        [JsonProperty("usuario")]
        public required string usuario { get; set; }
    }
}
