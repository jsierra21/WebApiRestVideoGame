using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using MediatR;

namespace Application.VideoStore.Commands
{
    public class VideoJuegosCommandHandler(
        IVideoJuegosService videojuegosService,
        IMapper mapper
        ) : IRequestHandler<VideoJuegosCommand, ResponseDTO>
    {
        private readonly IVideoJuegosService _videojuegosService = videojuegosService;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDTO> Handle(VideoJuegosCommand request, CancellationToken cancellationToken)
        {
            var fb = _mapper.Map<VideoJuegosDto>(request);
            return await _videojuegosService.RegistrarVideoJuegoService(fb);
        }
    }
}
