using Application.PushNotification.Commands;
using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Application.SQLContext.PushNotification.Mapping
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
        }
    }
}
