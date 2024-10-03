using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IVideoJuegosService
    {
        Task<ResponseDTO> RegistrarVideoJuegoService(VideoJuegosDto dto);
        Task<List<VideoJuegosEntity>> ListarVideoJuegosService();

    }
}
