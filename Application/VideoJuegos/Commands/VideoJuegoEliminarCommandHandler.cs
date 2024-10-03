using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.VideoStore.Commands
{
    public class VideoJuegoEliminarCommandHandler : IRequestHandler<EliminarVideoJuegoCommand, ResponseDTO>
    {
        private readonly IVideoJuegosService _videojuegosService;

        public VideoJuegoEliminarCommandHandler(IVideoJuegosService videojuegosService)
        {
            _videojuegosService = videojuegosService;
        }

        public async Task<ResponseDTO> Handle(EliminarVideoJuegoCommand request, CancellationToken cancellationToken)
        {
            return await _videojuegosService.EliminarVideoJuegoService(request.VideojuegoID);
        }
    }
}
