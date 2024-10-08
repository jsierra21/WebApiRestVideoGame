﻿using Core.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Application.VideoStore.Commands
{
    public class VideoJuegosCommand : IRequest<ResponseDTO>
    {
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
        public required string Usuario { get; set; }
    }

}
