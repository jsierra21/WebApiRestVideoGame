using Core.Interfaces;
using Core.Interfaces.store;
using Core.Interfaces.usuario;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Unidad de trabajo (unit of work) es el patrón que permite manejar transacciones durante la manipulación de datos utilizando el patrón repositorio. 
    /// Mantiene una lista de los objetos afectados por una transacción de negocio y coordina la escritura de los cambios 
    /// y la resolución de problemas de concurrencia
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbModelContext _context;

        //private readonly IPostRepository _postRepository; // Para cuando se hacen otras operaciones, a parte del CRUD (IEntityRepository)
        //private readonly IRepository<Entity> _userRepository; // Para entidades que sólo hacen CRUD
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IVideoJuegosRepository _videoJuegosRepository;

        private readonly Dictionary<Type, object> _storeProcedureRepositories = new Dictionary<Type, object>();


        public UnitOfWork(DbModelContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IUsuarioRepository UsuarioRepository => _usuarioRepository ?? new UsuarioRepository(_context);
        public IVideoJuegosRepository VideoJuegosRepository => _videoJuegosRepository ?? new VideoJuegosRepository(_context);

        //      public IAccountRepository AccountRepository => _accountRepository ?? new AccountRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
