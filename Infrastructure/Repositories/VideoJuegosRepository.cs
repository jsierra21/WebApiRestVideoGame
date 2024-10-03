using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.store;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class VideoJuegosRepository : IVideoJuegosRepository
    {
        private readonly DbModelContext _context;

        // Constructor que inyecta el contexto de base de datos
        public VideoJuegosRepository(DbModelContext context)
        {
            _context = context;
        }

        // Método para registrar un videojuego con validación de nombre único
        public async Task<VideoJuegosEntity> RegistrarVideoJuego(VideoJuegosDto dto)
        {
            try
            {
                // Validar si el videojuego ya está registrado
                var videoExistente = await _context.VideoJuegos
                    .FirstOrDefaultAsync(v => v.Nombre == dto.Nombre);

                if (videoExistente != null)
                {
                    throw new BusinessException("El Videojuego ya está registrado.");
                }

                // Crear el nuevo videojuego
                var video = new VideoJuegosEntity
                {
                    Nombre = dto.Nombre,
                    Compania = dto.Compania,
                    AnioLanzamiento = dto.AnioLanzamiento,
                    Precio = dto.Precio,
                    PuntajePromedio = 0m, // Valor inicial por defecto
                    Usuario = dto.Usuario, // Usuario que registra
                    FechaActualizacion = DateTime.Now // Fecha actual
                };

                // Guardar el videojuego en la base de datos
                _context.VideoJuegos.Add(video);
                await _context.SaveChangesAsync();

                return video; // Devuelve el videojuego registrado
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y lanza una nueva con el mensaje detallado
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
                    .FromSqlRaw("EXEC [dbo].[SpListarVideoJuegos]") // Ejecuta el procedimiento
                    .ToListAsync(); // Convertir los resultados a una lista

                return videojuegos; // Devuelve la lista de videojuegos
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y lanza una excepción personalizada con el mensaje de error
                throw new BusinessException("Error al devolver la consulta: " + ex.Message);
            }
        }
    }
}
