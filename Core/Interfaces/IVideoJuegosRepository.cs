

using Core.DTOs;

namespace Core.Interfaces.store
{
    public interface IVideoJuegosRepository
    {
        Task<List<VideoJuegosEntity>> ListarVideoJuegos();
        Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID);
        Task<VideoJuegosEntity> RegistrarVideoJuego(VideoJuegosDto video); // Acepta un objeto UsuarioEntity como parámetro
        Task<VideoJuegosEntity> ActualizarVideoJuego(VideoJuegosActualizarDto video); // Acepta un objeto UsuarioEntity como parámetro


    }
}
