using Core.Entities;
using Core.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
