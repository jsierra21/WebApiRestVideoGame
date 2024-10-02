using Application.PushNotification.Commands;
using AutoMapper;
using Core.DTOs;

namespace Application.SQLContext.PushNotification.Mapping
{
    internal class PushNotificationMapper : Profile
    {
        public PushNotificationMapper()
        {
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<AutomapperProfile>();
            //    cfg.AllowNullDestinationValues = false;
            //});

            //// Check that there are no issues with this configuration, which we'll encounter eventually at runtime.
            //config.AssertConfigurationIsValid();

            //config.CreateMapper();

            //CreateMap<Usuario, UsuarioDto>().ForMember(d => d.CodUsuario, a => a.Ignore());

            CreateMap<UsuarioCommand, UsuarioDto>().ReverseMap();

        }
    }
}
