using MediatR;

namespace Application.VideoStore.Queries
{
    public class ObtenerVideoJuegoPorIdQuery : IRequest<VideoJuegosEntity>
    {
        public int VideojuegoID { get; set; }  // El ID del videojuego que se va a consultar

        public ObtenerVideoJuegoPorIdQuery(int videojuegoID)
        {
            VideojuegoID = videojuegoID;
        }
    }
}
