using Core.DTOs;
using MediatR;

namespace Application.VideoStore.Queries;
public class ConsultaPaginadaVideoJuegosQuery : IRequest<PaginacionResponse<VideoJuegosEntity>>
{
    public int Pagina { get; set; }
    public int RegistrosPorPagina { get; set; }
}
