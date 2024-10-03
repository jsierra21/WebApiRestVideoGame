using Core.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Application.VideoStore.Commands
{
    public class UsuarioCommand : IRequest<ResponseDTO>
    {
        [JsonProperty("nombre_usuario")]
        public required string Nombre_usuario { get; set; }

        [JsonProperty("correo_electronico")]
        public required string Correo_electronico { get; set; }

        [JsonProperty("password")]
        public required string Password { get; set; }
    }
}
