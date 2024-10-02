using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using MediatR;

namespace Application.PushNotification.Commands
{
    public class UsuarioCommandHandler(
        IUsuarioService usuarioService,
        IMapper mapper
        ) : IRequestHandler<UsuarioCommand, ResponseDTO>
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDTO> Handle(UsuarioCommand request, CancellationToken cancellationToken)
        {
            var fb = _mapper.Map<UsuarioDto>(request);
            return await _usuarioService.RegistrarUsuarioNotificacion(fb);
        }
    }
}
