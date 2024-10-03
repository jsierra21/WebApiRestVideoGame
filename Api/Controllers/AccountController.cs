using Api.Responses;
using Api.ViewsProcess;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Interfaces;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IUsuarioService _usuarioService;


        public AccountController(IAccountService accountService,
             IMapper mapper,
             IConfiguration configuration,
              IPasswordService passwordService,
              IUsuarioService usuarioService
            )
        {
            _accountService = accountService;
            _mapper = mapper;
            _configuration = configuration;
            _passwordService = passwordService;
            _usuarioService = usuarioService;

        }

        /// <summary>
        /// Sistema de Logueo
        /// </summary>
        /// <param name="accountDto"></param>   
        /// <returns></returns>
        [HttpPost("Login", Name = "Login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] AccountLogin accountDto)
        {
            try
            {
                string password = Encoding.UTF8.GetString(Convert.FromBase64String(accountDto.Password));
                AccountLogin accountLogin = new AccountLogin()
                {
                    Password = password,
                    Correo = accountDto.Correo
                };

                AccountLoginValidator val = new();
                var validationResult = val.Validate(accountLogin);

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                //if it is a valid user
                var validation = await IsValidUserForAut(accountLogin);
                if (validation.Item1)
                {
                    TokenProcess tokenProcess = new TokenProcess(_configuration);
                    var token = tokenProcess.GenerateTokenUserWeb(validation.Item2);
                    UsuarioDto usuarioDto = _mapper.Map<UsuarioDto>(validation.Item2);

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        HttpContext.Response.Headers.Add("Authorization", token);
                        ResponseAuth r = new()
                        {
                            Status = true,
                            Message = "El usuario logueo correctamente",
                            UserName = usuarioDto.Nombre_usuario,
                            Correo = usuarioDto.Correo_electronico,
                            ExpiroClave = "true",
                            Token = "Bearer " + token
                        };
                        var response = new ApiResponse<ResponseAuth>(r, 200);
                        return Ok(response);
                    }
                    return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
                }

                return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar validar credenciales: {e.Message}");

            }
        }


        private async Task<(bool, UsuarioEntity)> IsValidUserForAut([FromBody] AccountLogin login)
        {

            var listuser = await _usuarioService.GetLoginByCredentialsUsuario(login);
            bool isValid = false;
            if (listuser != null)
            {
                isValid = _passwordService.Check(listuser.Password, login.Password);
            }
            return (isValid, listuser);
        }

    }
}
