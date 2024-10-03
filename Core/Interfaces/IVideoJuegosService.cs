using Core.DTOs;

namespace Core.Interfaces
{
    public interface IVideoJuegosService
    {
        Task<List<VideoJuegosEntity>> ListarVideoJuegosService();
        Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID);
        Task<ResponseDTO> RegistrarVideoJuegoService(VideoJuegosDto dto);
        Task<ResponseDTO> ActualizarVideoJuegoService(VideoJuegosActualizarDto dto);
        Task<ResponseDTO> EliminarVideoJuegoService(int videojuegoID);
        Task<int> CountVideoJuegos(); // Método para contar total de videojuegos
        Task<PaginacionResponse<VideoJuegosEntity>> ListarVideoJuegosPaginados(int pagina, int registrosPorPagina); // Método para listar videojuegos paginados
    }

}
