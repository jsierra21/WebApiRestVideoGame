

using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.usuario
{
    public interface IUsuarioRepository
    {
        Task<UsuarioEntity> RegistrarUsuario(UsuarioDto usuario); // Acepta un objeto UsuarioEntity como parámetro
        Task<UsuarioEntity> GetLoginByCredentialsAut(AccountLogin login);


    }
}
