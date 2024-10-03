using Application.VideoStore.Commands;
using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Application.SQLContext.VideoStore.Mapping
{
    internal class PerfilMapper : Profile
    {
        public PerfilMapper()
        {
            CreateMap<UsuarioCommand, UsuarioDto>().ReverseMap();
            // Configura el mapeo entre UsuarioEntity y UsuarioDto
            CreateMap<UsuarioEntity, UsuarioDto>();
            // Si necesitas mapear en la dirección inversa (de UsuarioDto a UsuarioEntity)
            CreateMap<UsuarioDto, UsuarioEntity>();


            CreateMap<VideoJuegosCommand, VideoJuegosDto>().ReverseMap();
            // Configura el mapeo entre VideoJuegosEntity y VideoJuegosDto
            CreateMap<VideoJuegosEntity, VideoJuegosDto>();
            // Si necesitas mapear en la dirección inversa (de VideoJuegosDto a VideoJuegosEntity)
            CreateMap<VideoJuegosDto, VideoJuegosEntity>();


            CreateMap<VideoJuegosActualizarCommand, VideoJuegosActualizarDto>().ReverseMap();
            // Configura el mapeo entre VideoJuegosEntity y UsuarioDto
            CreateMap<VideoJuegosEntity, VideoJuegosActualizarDto>();
            // Si necesitas mapear en la dirección inversa (de VideoJuegosDto a VideoJuegosEntity)
            CreateMap<VideoJuegosActualizarDto, VideoJuegosEntity>();
        }
    }
}
