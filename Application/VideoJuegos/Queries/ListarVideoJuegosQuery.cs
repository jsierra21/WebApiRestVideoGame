using MediatR;

namespace Application.VideoStore.Queries
{
    public class ListarVideoJuegosQuery : IRequest<List<VideoJuegosEntity>>
    {
    }
}
