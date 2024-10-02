using Core.DTOs;

namespace Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResponseDTO> RegistrarUsuarioNotificacion(UsuarioDto notification);
    }
}
