using Application.PushNotification.Commands;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(
        IMediator mediator,
        IPasswordService passwordService
        ) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPasswordService _passwordService = passwordService;

        // [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] UsuarioCommand post)
        {
            string passhas = _passwordService.Hash(post.Password);
            post.Password = passhas;
            var result = await _mediator.Send(post);

            if (result.Estado == 400)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
