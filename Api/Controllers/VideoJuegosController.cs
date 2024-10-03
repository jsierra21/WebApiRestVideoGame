using Application.VideoStore.Commands;
using Application.VideoStore.Queries; // Importar la consulta
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoJuegosController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Constructor que inyecta IMediator
        public VideoJuegosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Método para registrar un videojuego
        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> RegistrarVideoJuego([FromBody] VideoJuegosCommand command)
        {
            var result = await _mediator.Send(command); // Envía el comando de registro

            if (result.Estado == 400) // Verifica si hubo un error
            {
                return BadRequest(result); // Devuelve un error 400 si falla
            }

            return Ok(result); // Devuelve el resultado en caso de éxito
        }

        // Método para listar los videojuegos registrados
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarVideoJuegos()
        {
            // Envía la consulta al mediador para obtener los videojuegos
            var result = await _mediator.Send(new ListarVideoJuegosQuery());

            return Ok(result); // Devuelve los resultados en formato JSON
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVideoJuegoPorId(int id)
        {
            var query = new ObtenerVideoJuegoPorIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(); // Si no se encuentra el videojuego
            }

            return Ok(result); // Devuelve el videojuego encontrado
        }
    }
}
