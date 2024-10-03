using Core.DTOs;
using Core.Interfaces;

namespace Core.Services
{
    public class VideoJuegosService : IVideoJuegosService
    {

        private readonly IUnitOfWork _unitOfWork;
        public VideoJuegosService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     

        public async Task<List<VideoJuegosEntity>> ListarVideoJuegosService()
        {
            return await _unitOfWork.VideoJuegosRepository.ListarVideoJuegos();
        }

        public async Task<VideoJuegosEntity> ObtenerVideoJuegoPorIdService(int videojuegoID)
        {
            return await _unitOfWork.VideoJuegosRepository.ObtenerVideoJuegoPorIdService(videojuegoID);
        }

        public async Task<ResponseDTO> RegistrarVideoJuegoService(VideoJuegosDto dto)
        {
            ResponseDTO response = new();

            try
            {
                if (dto.nombre != null)
                {
                    VideoJuegosEntity result = await _unitOfWork.VideoJuegosRepository.RegistrarVideoJuego(dto);

                    response.Estado = 200;
                    response.Mensaje = "Video juego registrado  exitosamente.";
                    return response;

                }
                else
                {
                    response.Estado = 400;
                    response.Mensaje = "se ha generado un error no controlado en el servidor";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.Estado = 400;
                response.Mensaje = ex.Message;
                return response;
            }
        }


        public async Task<ResponseDTO> ActualizarVideoJuegoService(VideoJuegosActualizarDto dto)
        {
            ResponseDTO response = new();

            try
            {
                if (dto.nombre != null)
                {
                    VideoJuegosEntity result = await _unitOfWork.VideoJuegosRepository.ActualizarVideoJuego(dto);

                    response.Estado = 200;
                    response.Mensaje = "Video juego actualizado exitosamente.";
                    return response;

                }
                else
                {
                    response.Estado = 400;
                    response.Mensaje = "se ha generado un error no controlado en el servidor";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.Estado = 400;
                response.Mensaje = ex.Message;
                return response;
            }
        }

    }
}
