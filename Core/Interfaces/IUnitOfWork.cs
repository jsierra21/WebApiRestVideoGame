
namespace Core.Interfaces
{
    using Core.Interfaces.store;
    using Core.Interfaces.usuario;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interfaz encargada de indicar los atributos y métodos que la unidad de trabajo debe implementar
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        IUsuarioRepository UsuarioRepository { get; }
        IVideoJuegosRepository VideoJuegosRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
