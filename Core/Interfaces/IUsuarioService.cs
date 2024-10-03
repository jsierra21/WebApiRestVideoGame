using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResponseDTO> RegistrarUsuarioNotificacion(UsuarioDto notification);
        Task<UsuarioEntity> GetLoginByCredentialsUsuario(AccountLogin login);

    }
}
