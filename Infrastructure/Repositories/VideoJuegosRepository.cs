using Core.DTOs; // Importa los DTOs utilizados para las operaciones
using Core.Exceptions; // Importa las excepciones personalizadas
using Core.Interfaces.store; // Importa la interfaz del repositorio
using Infrastructure.Data; // Importa el contexto de la base de datos
using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core

namespace Infrastructure.Repositories
{
    public class VideoJuegosRepository : IVideoJuegosRepository
    {
        private readonly DbModelContext _context; // Contexto de la base de datos inyectado

        // Constructor que inyecta el contexto de base de datos
        public VideoJuegosRepository(DbModelContext context)
        {
            _context = context; // Inicializa el contexto de base de datos
        }

        // Método para registrar un videojuego con validación de nombre único
        public async Task<VideoJuegosEntity> RegistrarVideoJuego(VideoJuegosDto dto)
        {
            try
            {
                // Validar el DTO utilizando FluentValidation
                var validator = new VideoJuegosDtoValidator();
                var validationResult = await validator.ValidateAsync(dto);

                if (!validationResult.IsValid)
                {
                    // Lanzar excepción si hay errores de validación
                    throw new BusinessException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                // Validar si el videojuego ya está registrado en la base de datos
                var videoExistente = await _context.VideoJuegos
                    .FirstOrDefaultAsync(v => v.Nombre == dto.nombre);

                if (videoExistente != null)
                {
                    throw new BusinessException("El Videojuego ya está registrado.");
                }

                // Crear una nueva entidad de videojuego con los datos del DTO
                var video = new VideoJuegosEntity
                {
                    Nombre = dto.nombre,
                    Compania = dto.compania,
                    AnioLanzamiento = dto.anio_lanzamiento,
                    Precio = dto.precio,
                    PuntajePromedio = dto.puntaje_promedio, // Valor inicial por defecto para puntaje promedio
                    Usuario = dto.usuario, // Usuario que registra el videojuego
                    FechaActualizacion = DateTime.Now // Asigna la fecha y hora actual
                };

                // Añadir el nuevo videojuego al contexto de la base de datos
                _context.VideoJuegos.Add(video);
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

                return video; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el videojuego: " + ex.Message);
            }
        }
        // Método para listar videojuegos por medio de un procedimiento almacenado
        public async Task<List<VideoJuegosEntity>> ListarVideoJuegos()
        {
            try
            {
                // Ejecutar el procedimiento almacenado para listar videojuegos
                var videojuegos = await _context.VideoJuegos
                    .FromSqlRaw("EXEC [dbo].[SpListarVideoJuegos]") // Ejecuta el procedimiento almacenado
                    .ToListAsync(); // Convierte los resultados a una lista

                return videojuegos; // Devuelve la lista de videojuegos
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al devolver la consulta: " + ex.Message);
            }
        }

        // Método para obtener un videojuego por ID
        public async Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID)
        {
            try
            {
                // Busca el videojuego en la base de datos por su ID
                var videojuego = await _context.VideoJuegos
                    .FirstOrDefaultAsync(v => v.VideojuegoID == videojuegoID);

                // Si no se encuentra el videojuego, lanzar una excepción personalizada
                if (videojuego == null)
                {
                    throw new BusinessException("El Videojuego no se encontró.");
                }

                return videojuego; // Devuelve el videojuego encontrado
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener el videojuego: " + ex.Message);
            }
        }
    }
}
