using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using MediatR;

namespace Application.VideoStore.Commands
{
    public class VideoJuegosActualizarCommandHandler(
        IVideoJuegosService videojuegosService,
        IMapper mapper
        ) : IRequestHandler<VideoJuegosActualizarCommand, ResponseDTO>
    {
        private readonly IVideoJuegosService _videojuegosService = videojuegosService;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDTO> Handle(VideoJuegosActualizarCommand request, CancellationToken cancellationToken)
        {
            var fb = _mapper.Map<VideoJuegosActualizarDto>(request);
            return await _videojuegosService.ActualizarVideoJuegoService(fb);
        }
    }
}
