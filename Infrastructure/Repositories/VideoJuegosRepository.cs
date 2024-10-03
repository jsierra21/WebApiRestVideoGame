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

        public async Task<VideoJuegosEntity> ActualizarVideoJuego(VideoJuegosActualizarDto dto)
        {
            try
            {
                // Validar el DTO utilizando FluentValidation
                var validator = new VideoJuegosActualizarDtoValidator();
                var validationResult = await validator.ValidateAsync(dto);

                if (!validationResult.IsValid)
                {
                    throw new BusinessException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                // Buscar el videojuego existente en la base de datos
                var videojuegoExistente = await _context.VideoJuegos
                    .FirstOrDefaultAsync(v => v.VideojuegoID == dto.video_juego_id); // Asegúrate de que dto tenga VideojuegoID

                if (videojuegoExistente == null)
                {
                    throw new BusinessException("El Videojuego no se encontró.");
                }

                // Actualizar los campos del videojuego existente
                videojuegoExistente.Nombre = dto.nombre;
                videojuegoExistente.Compania = dto.compania;
                videojuegoExistente.AnioLanzamiento = dto.anio_lanzamiento;
                videojuegoExistente.Precio = dto.precio;
                videojuegoExistente.PuntajePromedio = dto.puntaje_promedio; // Este puede ser actualizado según la lógica de negocio
                videojuegoExistente.Usuario = dto.usuario; // Asignar el usuario que actualiza
                videojuegoExistente.FechaActualizacion = DateTime.Now; // Asigna la fecha y hora actual

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return videojuegoExistente; // Devuelve el videojuego actualizado
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el videojuego: " + ex.Message);
            }
        }

        public async Task<VideoJuegosEntity> EliminarVideoJuego(int videojuegoID)
        {
            try
            {
                var videojuego = await _context.VideoJuegos
                .FirstOrDefaultAsync(v => v.VideojuegoID == videojuegoID);

            if (videojuego == null)
            {
                throw new BusinessException("El Videojuego no se encontró.");
            }

            _context.VideoJuegos.Remove(videojuego);
            await _context.SaveChangesAsync();

            return videojuego; // Devuelve el videojuego eliminado
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los videojuegos paginados: " + ex.Message);
            }
        }

        public async Task<PaginacionResponse<VideoJuegosEntity>> ListarVideoJuegosPaginados(int pageNumber, int pageSize)
        {
            try
            {
                // Total de registros en la base de datos
                var totalCount = await _context.VideoJuegos.CountAsync();

                // Total de páginas
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Verificar si la página actual tiene registros
                var hasNextPage = pageNumber < totalPages;
                var hasPreviousPage = pageNumber > 1;

                // Obtener los registros de la página actual
                var items = await _context.VideoJuegos
                    .Skip((pageNumber - 1) * pageSize) // Saltar los registros de las páginas anteriores
                    .Take(pageSize) // Tomar solo el tamaño de página especificado
                    .ToListAsync();

                // Crear la respuesta de paginación
                return new PaginacionResponse<VideoJuegosEntity>
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    Items = items
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los videojuegos paginados: " + ex.Message);
            }
        }


        public async Task<int> CountVideoJuegos()
        {
            return await _context.VideoJuegos.CountAsync(); // Cuenta el total de videojuegos en la base de datos
        }


    }
}
