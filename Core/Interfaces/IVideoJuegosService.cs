using Core.DTOs;

namespace Core.Interfaces
{
    public interface IVideoJuegosService
    {
        Task<ResponseDTO> RegistrarVideoJuegoService(VideoJuegosDto dto);
        Task<List<VideoJuegosEntity>> ListarVideoJuegosService();
        Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID);


    }
}
