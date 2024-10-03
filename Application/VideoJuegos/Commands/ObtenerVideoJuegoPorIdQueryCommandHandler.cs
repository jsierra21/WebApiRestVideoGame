using Core.Interfaces;
using MediatR;

namespace Application.VideoStore.Queries
{
    // Query para obtener videojuego por ID
    public class ObtenerVideoJuegoPorIdQueryCommandHandler : IRequest<VideoJuegosEntity>
    {
        public int VideojuegoID { get; set; }  // El ID del videojuego que se va a consultar

        public ObtenerVideoJuegoPorIdQueryCommandHandler(int videojuegoID)
        {
            VideojuegoID = videojuegoID;
        }
    }

    // Handler para procesar la consulta por ID
    public class ObtenerVideoJuegoPorIdHandler : IRequestHandler<ObtenerVideoJuegoPorIdQuery, VideoJuegosEntity>
    {
        private readonly IVideoJuegosService _videojuegosService;

        public ObtenerVideoJuegoPorIdHandler(IVideoJuegosService videojuegosService)
        {
            _videojuegosService = videojuegosService;
        }

        public async Task<VideoJuegosEntity> Handle(ObtenerVideoJuegoPorIdQuery request, CancellationToken cancellationToken)
        {
            // Llama al servicio para obtener el videojuego por ID
            return await _videojuegosService.ObtenerVideoJuegoPorIdService(request.VideojuegoID);
        }
    }
}
