using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.store;

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
                // Valida el dto según tu lógica (por ejemplo, que todos los campos requeridos estén presentes)
                if (dto == null || string.IsNullOrWhiteSpace(dto.nombre) || dto.anio_lanzamiento <= 0)
                {
                    return new ResponseDTO { Estado = 400, Mensaje = "Datos inválidos." };
                }

                if (dto.nombre != null)
                {
                    VideoJuegosEntity result = await _unitOfWork.VideoJuegosRepository.RegistrarVideoJuego(dto);
                    if (result.Nombre.Length > 0)
                    {
                        response.Estado = 200;
                        response.Mensaje = "Video juego registrado  exitosamente.";
                        return response;
                    }
                    else {
                        response.Estado = 400;
                        response.Mensaje = "se ha generado un error no controlado en el servidor";
                        return response;
                    }
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

            if (dto == null || dto.video_juego_id <= 0 || string.IsNullOrWhiteSpace(dto.nombre))
            {
                return new ResponseDTO { Estado = 400, Mensaje = "El videojuego no existe o los datos son inválidos." };
            }

            VideoJuegosEntity result = await _unitOfWork.VideoJuegosRepository.ActualizarVideoJuego(dto);

            if (result == null)
            {
                return new ResponseDTO { Estado = 404, Mensaje = "El videojuego no existe o los datos son inválidos." };
            }

            return new ResponseDTO { Estado = 200, Mensaje = "Videojuego actualizado exitosamente." };
        }


        public async Task<ResponseDTO> EliminarVideoJuegoService(int videojuegoID)
        {
            ResponseDTO response = new();

            try
            {
                if (videojuegoID != 0)
                {
                    VideoJuegosEntity result = await _unitOfWork.VideoJuegosRepository.EliminarVideoJuego(videojuegoID);

                    response.Estado = 200;
                    response.Mensaje = "Video juego eliminado exitosamente.";
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

        public async Task<int> CountVideoJuegos()
        {
            return await _unitOfWork.VideoJuegosRepository.CountVideoJuegos(); // Método que contar videojuegos en el repositorio
        }

        public async Task<PaginacionResponse<VideoJuegosEntity>> ListarVideoJuegosPaginados(int pagina, int registrosPorPagina)
        {
            // Llama al repositorio para obtener la respuesta de paginación
            var resultado = await _unitOfWork.VideoJuegosRepository.ListarVideoJuegosPaginados(pagina, registrosPorPagina);
            return resultado; // Devuelve la respuesta de paginación
        }



    }
}
