using Application.VideoStore.Commands;
using Application.VideoStore.Queries;
using Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization; // Importar el espacio de nombres para autorización
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Asegura que todos los métodos requieren un token JWT
    public class VideoJuegosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

        public VideoJuegosController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarVideoJuegos()
        {
            const string cacheKey = "videojuegosList";

            if (!_cache.TryGetValue(cacheKey, out List<VideoJuegosEntity> videojuegos))
            {
                videojuegos = await _mediator.Send(new ListarVideoJuegosQuery());

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                _cache.Set(cacheKey, videojuegos, cacheEntryOptions);
            }

            return Ok(videojuegos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVideoJuegoPorId(int id)
        {
            var query = new ObtenerVideoJuegoPorIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> RegistrarVideoJuego([FromBody] VideoJuegosCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Estado == 400)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public async Task<IActionResult> ActualizarVideoJuego(int id, [FromBody] VideoJuegosActualizarCommand command)
        {
            command.video_juego_id = id;

            var result = await _mediator.Send(command);

            if (result.Estado == 400)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("eliminar/{videojuegoID}")]
        public async Task<IActionResult> EliminarVideoJuego(int videojuegoID)
        {
            var result = await _mediator.Send(new EliminarVideoJuegoCommand { VideojuegoID = videojuegoID });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("listar/paginado")]
        public async Task<IActionResult> ListarVideoJuegosPaginados([FromQuery] PaginacionDto paginacionDto)
        {
            var result = await _mediator.Send(new ConsultaPaginadaVideoJuegosQuery
            {
                Pagina = paginacionDto.Pagina,
                RegistrosPorPagina = paginacionDto.RegistrosPorPagina
            });

            return Ok(result);
        }
    }
}
