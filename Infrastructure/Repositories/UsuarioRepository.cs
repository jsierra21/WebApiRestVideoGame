using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.usuario;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbModelContext _context;

        public UsuarioRepository(DbModelContext context)
        {
            _context = context;
        }

        // Método para registrar usuario usando un procedimiento almacenado
        public async Task<UsuarioEntity> RegistrarUsuario(UsuarioDto dto)
        {

            try
            {
                // Llamada al procedimiento almacenado con parámetros
                var parameters = new[]
                {
                    new SqlParameter("@Usr_nombre_usuario", dto.Nombre_usuario),
                    new SqlParameter("@Usr_correo_electronico", dto.Correo_electronico),
                    new SqlParameter("@Usr_Password", dto.Password)
                    // Agrega más parámetros según tu lógica
                };

                // Ejecutar el procedimiento almacenado
                await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[RegistrarUserSp] @Usr_nombre_usuario, @Usr_correo_electronico, @Usr_Password", parameters);

                // Crear y devolver un nuevo UsuarioEntity
                var usuario = new UsuarioEntity
                {
                    NombreUsuario = dto.Nombre_usuario,
                    CorreoElectronico = dto.Correo_electronico,
                    Password = dto.Password,
                    // Asigna otros campos si es necesario
                };

                return usuario; // Devuelve el usuario registrado
            }
            catch (Exception ex)
            {
                // Maneja la excepción (puedes registrar el error o lanzar una excepción personalizada)
                throw new Exception("Error al registrar el usuario: " + ex.Message);
            }
        }

        public async Task<UsuarioEntity> GetLoginByCredentialsAut(AccountLogin dto)
        {
            try
            {
                // Parámetros para el procedimiento almacenado
                var parameters = new[]
                {
                    new SqlParameter("@Usr_correo_electronico", dto.Correo),
                    new SqlParameter("@Usr_Password", dto.Password)
                };

                // Ejecutar el procedimiento almacenado y evitar la composición del lado del servidor
                var usuarios =  _context.Usuarios
                    .FromSqlRaw("EXEC [dbo].[SpAutenticacion] @Usr_correo_electronico, @Usr_Password", parameters)
                    .AsEnumerable() // Ejecuta el procedimiento y luego trabaja con los datos en memoria
                    .ToList();

                // Verificar si se encontraron resultados
                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                {
                    throw new BusinessException("Credenciales inválidas");
                }

                return usuario; // Devuelve el usuario encontrado
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                throw new BusinessException("Error al autenticar el usuario: " + ex.Message);
            }
        }


    }
}
