

using Core.DTOs;

namespace Core.Interfaces.store
{
    public interface IVideoJuegosRepository
    {
        Task<List<VideoJuegosEntity>> ListarVideoJuegos();
        Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID);
        Task<VideoJuegosEntity> RegistrarVideoJuego(VideoJuegosDto video); // Acepta un objeto UsuarioEntity como parámetro
        Task<VideoJuegosEntity> ActualizarVideoJuego(VideoJuegosActualizarDto video); // Acepta un objeto UsuarioEntity como parámetro
        Task<VideoJuegosEntity> EliminarVideoJuego(int videojuegoID); // Acepta un objeto UsuarioEntity como parámetro
        Task<int> CountVideoJuegos(); // Método para contar total de videojuegos
        Task<PaginacionResponse<VideoJuegosEntity>> ListarVideoJuegosPaginados(int pageNumber, int pageSize);

    }
}
