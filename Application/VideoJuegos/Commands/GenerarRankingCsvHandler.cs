using Core.DTOs;
using Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class GenerarRankingCsvHandler : IRequestHandler<GenerarRankingCsvQuery, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public GenerarRankingCsvHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(GenerarRankingCsvQuery request, CancellationToken cancellationToken)
    {
        // Validar el valor de Top
        if (request.Top <= 0)
        {
            throw new ArgumentException("El valor de 'top' no es válido.");
        }

        int top = request.Top > 0 ? request.Top : 20;


        // Obtener todos los videojuegos
        var videojuegos = await _unitOfWork.VideoJuegosRepository.ObtenerTodosVideoJuegosAsync();

        // Generar el ranking
        var ranking = videojuegos
            .Select(v => new VideoJuegoRankingDto
            {
                Nombre = v.Nombre,
                Compania = v.Compania,
                PuntajePromedio = v.PuntuacionPromedio, // Suponiendo que ya no utilizas Calificaciones
                Clasificacion = (videojuegos.IndexOf(v) < top / 2) ? "GOTY" : "AAA" // Clasificación según el índice
            })
            .OrderByDescending(v => v.PuntajePromedio) // Ordenar por puntaje promedio
            .Take(top) // Tomar el número requerido
            .ToList();

        // Construir el CSV
        var sb = new StringBuilder();
        sb.AppendLine("Nombre|Compania|Puntaje|Clasificacion"); // Encabezado

        foreach (var videojuego in ranking)
        {
            sb.AppendLine($"{videojuego.Nombre}|{videojuego.Compania}|{videojuego.PuntajePromedio}|{videojuego.Clasificacion}");
        }

        return sb.ToString();
    }


}
