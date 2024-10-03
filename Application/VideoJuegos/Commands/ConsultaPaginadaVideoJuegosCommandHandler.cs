using Application.VideoStore.Queries;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using MediatR;

public class ConsultaPaginadaVideoJuegosCommandHandler : IRequestHandler<ConsultaPaginadaVideoJuegosQuery, PaginacionResponse<VideoJuegosEntity>>
{
    private readonly IVideoJuegosService _videojuegosService;

    public ConsultaPaginadaVideoJuegosCommandHandler(IVideoJuegosService videojuegosService)
    {
        _videojuegosService = videojuegosService;
    }

    public async Task<PaginacionResponse<VideoJuegosEntity>> Handle(ConsultaPaginadaVideoJuegosQuery request, CancellationToken cancellationToken)
    {
        // Obtener el total de registros de videojuegos
        var totalRegistros = await _videojuegosService.CountVideoJuegos();

        // Obtener los videojuegos paginados
        var items = await _videojuegosService.ListarVideoJuegosPaginados(request.Pagina, request.RegistrosPorPagina);

        // Crear la respuesta de paginación
        return new PaginacionResponse<VideoJuegosEntity>
        {
            Items = items.Items,
            TotalCount = totalRegistros,
            PageSize = request.RegistrosPorPagina,
            CurrentPage = request.Pagina,
            TotalPages = (int)Math.Ceiling((double)totalRegistros / request.RegistrosPorPagina),
            HasNextPage = request.Pagina < (int)Math.Ceiling((double)totalRegistros / request.RegistrosPorPagina),
            HasPreviousPage = request.Pagina > 1
        };
    }


}
