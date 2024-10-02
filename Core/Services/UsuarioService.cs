using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUnitOfWork _unitOfWork;
        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        public async Task<ResponseDTO> RegistrarUsuarioNotificacion(UsuarioDto notificationModel)
        {
            ResponseDTO response = new();

            try
            {
                if (notificationModel.Correo_electronico != null)
                {
                    // Inicializar la conexión a Firebase
                    // Construir el objeto a enviar en la notificación
                    //  FirebaseMessage payload = ConstruirModeloNotificacion(notificationModel);

                    // Enviar notificaciónnotificationModel
                    UsuarioEntity result = await _unitOfWork.UsuarioRepository.RegistrarUsuario(notificationModel);

                    response.Estado = 200;
                    response.Mensaje = "Notificación enviada exitosamente.";
                    return response;

                }
                else
                {
                    response.Estado = 400;
                    response.Mensaje = "No se encontró un dispositivo válido.";
                    return response;
                }

                // Code here for APN Sender (iOS Device)
                // var apn = new ApnSender(apnSettings, httpClient);
                // await apn.SendAsync(notification, deviceToken);
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
