
namespace Core.Interfaces
{
    using Core.Interfaces.usuario;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interfaz encargada de indicar los atributos y métodos que la unidad de trabajo debe implementar
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        
        IUsuarioRepository UsuarioRepository { get; }

      //  IStoreProcedureRepository<T> StoreProcedure<T>() where T : class;


        //SGD
        #region Excel Generador Repositories
        #endregion
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
