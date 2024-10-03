using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.usuario;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbModelContext _context;

        // Constructor que inyecta el contexto de base de datos
        public UsuarioRepository(DbModelContext context)
        {
            _context = context;
        }

        // Método para registrar usuario con validación de correo único
        public async Task<UsuarioEntity> RegistrarUsuario(UsuarioDto dto)
        {
            try
            {
                // Validar si el correo ya está registrado
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == dto.Correo_electronico);

                // Si se encuentra un usuario con el mismo correo, lanzar excepción
                if (usuarioExistente != null)
                {
                    throw new BusinessException("El correo electrónico ya está registrado.");
                }

                // Configurar los parámetros del procedimiento almacenado con los valores del DTO
                var parameters = new[]
                {
                    new SqlParameter("@Usr_nombre_usuario", dto.Nombre_usuario),   // Nombre del usuario
                    new SqlParameter("@Usr_correo_electronico", dto.Correo_electronico), // Correo electrónico del usuario
                    new SqlParameter("@Usr_Password", dto.Password)   // Contraseña del usuario
                };

                // Ejecutar el procedimiento almacenado para registrar el usuario
                await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[SpRegistrarUser] @Usr_nombre_usuario, @Usr_correo_electronico, @Usr_Password", parameters);

                // Crear y devolver una nueva instancia de UsuarioEntity
                var usuario = new UsuarioEntity
                {
                    NombreUsuario = dto.Nombre_usuario,   // Asignar el nombre del usuario
                    CorreoElectronico = dto.Correo_electronico, // Asignar el correo electrónico
                    Password = dto.Password,  // Asignar la contraseña
                };

                return usuario; // Devuelve el usuario registrado
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y lanza una nueva con el mensaje detallado
                throw new Exception("Error al registrar el usuario: " + ex.Message);
            }
        }

        // Método para obtener un usuario por sus credenciales usando un procedimiento almacenado de autenticación
        public async Task<UsuarioEntity> GetLoginByCredentialsAut(AccountLogin dto)
        {
            try
            {
                // Configurar los parámetros del procedimiento almacenado con las credenciales del DTO (Correo y Contraseña)
                var parameters = new[]
                {
                    new SqlParameter("@Usr_correo_electronico", dto.Correo),   // Correo electrónico del usuario
                    new SqlParameter("@Usr_Password", dto.Password)   // Contraseña del usuario
                };

                // Ejecutar el procedimiento almacenado para autenticar al usuario y convertir los resultados en una lista
                var usuarios = _context.Usuarios
                    .FromSqlRaw("EXEC [dbo].[SpAutenticacion] @Usr_correo_electronico, @Usr_Password", parameters) // Ejecuta el procedimiento
                    .AsEnumerable()   // Carga los resultados en memoria para evitar problemas de composición de consultas
                    .ToList();  // Convertir los resultados a una lista

                // Tomar el primer usuario de la lista de resultados
                var usuario = usuarios.FirstOrDefault();

                // Si no se encontró ningún usuario, lanzar una excepción personalizada
                if (usuario == null)
                {
                    throw new BusinessException("Credenciales inválidas");
                }

                return usuario; // Devuelve el usuario autenticado si las credenciales son válidas
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción durante la autenticación y lanza una excepción personalizada con el mensaje de error
                throw new BusinessException("Error al autenticar el usuario: " + ex.Message);
            }
        }
    }
}
