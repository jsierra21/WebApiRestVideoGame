

using Core.DTOs;

namespace Core.Interfaces.store
{
    public interface IVideoJuegosRepository
    {
        Task<VideoJuegosEntity> RegistrarVideoJuego(VideoJuegosDto video); // Acepta un objeto UsuarioEntity como parámetro
        Task<List<VideoJuegosEntity>> ListarVideoJuegos();
        Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID);



    }
}
