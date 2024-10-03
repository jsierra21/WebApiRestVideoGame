using Core.Interfaces;
using MediatR;

namespace Application.VideoStore.Queries
{
    public class ListarVideoJuegosCommandHandler : IRequestHandler<ListarVideoJuegosQuery, List<VideoJuegosEntity>>
    {
        private readonly IVideoJuegosService _videojuegosService;

        public ListarVideoJuegosCommandHandler(IVideoJuegosService videojuegosService)
        {
            _videojuegosService = videojuegosService;
        }

        public async Task<List<VideoJuegosEntity>> Handle(ListarVideoJuegosQuery request, CancellationToken cancellationToken)
        {
            return await _videojuegosService.ListarVideoJuegosService();
        }
    }
}
