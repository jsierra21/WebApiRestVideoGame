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
                    UsuarioEntity result = await _unitOfWork.UsuarioRepository.RegistrarUsuario(notificationModel);

                    response.Estado = 200;
                    response.Mensaje = "Usuario registrado  exitosamente.";
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

        public async Task<UsuarioEntity> GetLoginByCredentialsUsuario(AccountLogin login)
        {
            return await _unitOfWork.UsuarioRepository.GetLoginByCredentialsAut(login);
        }

    }
}
