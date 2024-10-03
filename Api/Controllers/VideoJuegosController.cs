﻿using Application.VideoStore.Commands;
using Application.VideoStore.Queries; // Importar la consulta
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
// Asegúrate de incluir el espacio de nombres correspondiente

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoJuegosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache; // Añade una variable para la caché


        // Constructor que inyecta IMediator
        public VideoJuegosController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache; // Inicializa la caché

        }

        // Método para registrar un videojuego
        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> RegistrarVideoJuego([FromBody] VideoJuegosCommand command)
        {
            var result = await _mediator.Send(command); // Envía el comando de registro

            if (result.Estado == 400)
            {
                return BadRequest(result); // Devuelve un error 400 si falla
            }

            return Ok(result); 
        }

        // Método para listar los videojuegos registrados con caché en memoria
        [HttpGet]
        [Route("listar")]
        public async Task<IActionResult> ListarVideoJuegos()
        {
            // Define la clave de caché
            const string cacheKey = "videojuegosList";

            // Intenta obtener los videojuegos de la caché
            if (!_cache.TryGetValue(cacheKey, out List<VideoJuegosEntity> videojuegos))
            {
                // Si no están en caché, envía la consulta al mediador para obtener los videojuegos
                videojuegos = await _mediator.Send(new ListarVideoJuegosQuery());

                // Configura las opciones de la caché
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    // Establece el tiempo de duración de la caché
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), //  10 minutos
                    SlidingExpiration = TimeSpan.FromMinutes(5) // 5 minutos
                };

                // Guarda los videojuegos en caché
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
                return NotFound(); // Si no se encuentra el videojuego
            }

            return Ok(result); 
        }
    }
}
