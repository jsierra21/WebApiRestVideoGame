using Api.Responses;
using Core.Entities;
using Core.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.ViewsProcess
{
    public class TokenProcess
    {
        private readonly IConfiguration _configuration;

        public TokenProcess(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateTokenUserWeb(UsuarioEntity usuario)
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //   Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim("Usuario", usuario.CorreoElectronico),
                new Claim("ID", usuario.IdUsuario.ToString())
            };

            //  Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Authentication:ExpireToken"]))
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //claves_eguimiento_token: seguimientotokenasdf644654sdaf6
    }

    public class ShouldBeAnAdminRequirement : IAuthorizationRequirement
    {
        // propiedades que se quieran utilizar para validar
    }

    public class ShouldBeAnAdminRequirementHandler : AuthorizationHandler<ShouldBeAnAdminRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;

        public ShouldBeAnAdminRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldBeAnAdminRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            string error = string.Empty;
            int codigoResponse = 200;

            // Validamos si no se envió un token para enseguida enviar el flujo a OnChallenge en startup.cs
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                await Task.CompletedTask;
                return;
            }

            // Obtenemos data del usuario, a través del Token
            if (!(context.User is null))
            {
                if (context.User.HasClaim(x => x.Type == "Usuario"))
                {
                    Claim claim = context.User.Claims.FirstOrDefault(x => x.Type == "Usuario");
                    string CodUser = claim.Value;
                    if (!httpContext.Items.ContainsKey("CodUser"))
                    {
                        httpContext.Items.Add("CodUser", CodUser);
                    }
                }

                if (context.User.HasClaim(x => x.Type == "ID"))
                {
                    Claim claim = context.User.Claims.FirstOrDefault(x => x.Type == "ID");
                    string UserID = claim.Value;
                    if (!httpContext.Items.ContainsKey("UserID"))
                    {
                        httpContext.Items.Add("UserID", UserID);
                    }
                }

                if (context.User.HasClaim(x => x.Type == ClaimTypes.Email))
                {
                    Claim claim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                    string UserEmail = claim.Value;
                    if (!httpContext.Items.ContainsKey("UserEmail"))
                    {
                        httpContext.Items.Add("UserEmail", UserEmail);
                    }
                }

                if (context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                {
                    Claim claim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                    string UserRole = claim.Value;
                    if (!httpContext.Items.ContainsKey("UserRole"))
                    {
                        httpContext.Items.Add("UserRole", UserRole);
                    }
                }
            }


            await Task.CompletedTask;
            return;
        }
    }
}
