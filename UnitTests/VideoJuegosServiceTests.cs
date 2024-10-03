using Xunit;
using FluentAssertions;
using Moq; 
using Core.DTOs;
using Core.Services;
using Core.Interfaces;
using Core.Interfaces.store;
using Microsoft.Extensions.Caching.Memory;
using MediatR;
using Application.VideoStore.Queries; // Reemplaza con el espacio de nombres correcto de tu proyecto
namespace UnitTests;
public class VideoJuegosServiceTests
{
    private readonly VideoJuegosService _service;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IVideoJuegosRepository> _mockVideoJuegosRepository;
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly Mock<IMediator> _mockMediator;


    public VideoJuegosServiceTests()
    {
        // Crea mocks
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockVideoJuegosRepository = new Mock<IVideoJuegosRepository>();
        _mockCache = new Mock<IMemoryCache>();
        _mockMediator = new Mock<IMediator>();

        // Configura el mock para el repositorio
        _mockUnitOfWork.Setup(uow => uow.VideoJuegosRepository)
            .Returns(_mockVideoJuegosRepository.Object);

        // Inicializa el servicio pasando el mock
        _service = new VideoJuegosService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task RegistrarVideoJuego_ShouldReturnSuccess_WhenValidData()
    {
        // Arrange
        var dto = new VideoJuegosDto
        {
            nombre = "Test Game",
            compania = "Test Company",
            anio_lanzamiento = 2024,
            precio = 124,
            puntaje_promedio = 1,
            usuario = "testuser"
        };

        var response = new ResponseDTO { Estado = 200, Mensaje = "Videojuego registrado exitosamente." };

        // Configura el mock para devolver una entidad simulada
        _mockUnitOfWork
            .Setup(uow => uow.VideoJuegosRepository.RegistrarVideoJuego(It.IsAny<VideoJuegosDto>()))
            .ReturnsAsync(new VideoJuegosEntity
            {
                VideojuegoID = 1,
                Nombre = dto.nombre,
                Compania = dto.compania,
                AnioLanzamiento = dto.anio_lanzamiento,
                Precio = dto.precio,
                PuntajePromedio = dto.puntaje_promedio
            });

        // Act
        var result = await _service.RegistrarVideoJuegoService(dto);

        // Assert
        Assert.Equal(200, result.Estado); // Asegúrate de que el resultado es 200
    }

    [Fact]
    public async Task RegistrarVideoJuego_ShouldReturnFailure_WhenInvalidData()
    {
        // Arrange: Creas un DTO con datos inválidos (por ejemplo, nombre nulo o año de lanzamiento inválido)
        var dto = new VideoJuegosDto
        {
            nombre = null,  // Nombre inválido
            compania = "Test Company",
            anio_lanzamiento = -1,  // Año inválido
            precio = 124,
            puntaje_promedio = 1,
            usuario = "testuser"
        };

        var result = await _service.RegistrarVideoJuegoService(dto);
        Assert.Equal(400, result.Estado);  // Verifica que el estado es 400
        Assert.Equal("Datos inválidos.", result.Mensaje);  // Verifica que el mensaje es el correcto
    }


    [Fact]
    public async Task ActualizarVideoJuego_ShouldReturnSuccess_WhenValidData()
    {
        // Arrange
        var dto = new VideoJuegosActualizarDto
        {
            video_juego_id = 1, // ID válido
            nombre = "Updated Test Game",
            compania = "Updated Company",
            anio_lanzamiento = 2025,
            precio = 150,
            puntaje_promedio = 4,
            usuario = "testuser"
        };

        // Configura el mock para devolver una entidad simulada
        _mockUnitOfWork
            .Setup(uow => uow.VideoJuegosRepository.ActualizarVideoJuego(It.IsAny<VideoJuegosActualizarDto>()))
            .ReturnsAsync(new VideoJuegosEntity
            {
                VideojuegoID = dto.video_juego_id,
                Nombre = dto.nombre,
                Compania = dto.compania,
                AnioLanzamiento = dto.anio_lanzamiento,
                Precio = dto.precio,
                PuntajePromedio = dto.puntaje_promedio
            });

        // Act
        var result = await _service.ActualizarVideoJuegoService(dto);
        Console.WriteLine($"ActualizarVideoJuegoService - DTO: {result.Mensaje}");

        // Assert
        Assert.Equal(200, result.Estado); // Asegúrate de que el resultado es 200
        Assert.Equal("Videojuego actualizado exitosamente.", result.Mensaje); // Verifica el mensaje
    }


    [Fact]
    public async Task ActualizarVideoJuego_ShouldReturnFailure_WhenInvalidData()
    {
        // Arrange
        var dto = new VideoJuegosActualizarDto
        {
            video_juego_id = 22, // ID inválido o no existente
            nombre = "", // Nombre inválido
            compania = "Updated Company",
            anio_lanzamiento = 0, // Año de lanzamiento inválido
            precio = -1, // Precio inválido
            puntaje_promedio = 4,
            usuario = "testuser"
        };

        // Configura el mock para simular una respuesta de error cuando el ID es inválido
        var expectedResponse = new ResponseDTO
        {
            Estado = 400, // Simula un error de validación
            Mensaje = "El videojuego no existe o los datos son inválidos."
        };

        _mockUnitOfWork
            .Setup(uow => uow.VideoJuegosRepository.ActualizarVideoJuego(It.IsAny<VideoJuegosActualizarDto>()))
            .ReturnsAsync((VideoJuegosEntity)null); // Simula que el videojuego no existe en la base de datos

        // Act
        var result = await _service.ActualizarVideoJuegoService(dto);
        Console.WriteLine($"ActualizarVideoJuegoService - DTO: {result.Mensaje}");

        // Assert
        result.Estado.Should().Be(400); // Espera un error de 400
        result.Mensaje.Should().Be(expectedResponse.Mensaje); // Verifica el mensaje de error
    }


    [Fact]
    public async Task ListarVideoJuegos_ShouldReturnFromCache_WhenCacheExists()
    {
        // Arrange
        var videojuegos = new List<VideoJuegosEntity>
            {
                new VideoJuegosEntity { VideojuegoID = 1, Nombre = "Test Game 1" },
                new VideoJuegosEntity { VideojuegoID = 2, Nombre = "Test Game 2" }
            };

        object cacheValue = videojuegos;
        _mockCache
            .Setup(c => c.TryGetValue("videojuegosList", out cacheValue))
            .Returns(true); // Simula que hay datos en la caché

        // Act
        var result = await _service.ListarVideoJuegosService();

        // Assert
        result.Should().BeEquivalentTo(videojuegos);
    }


    [Fact]
    public async Task ListarVideoJuegos_ShouldReturnFromMediator_WhenCacheDoesNotExist()
    {
        // Arrange
        var videojuegos = new List<VideoJuegosEntity>
            {
                new VideoJuegosEntity { VideojuegoID = 1, Nombre = "Test Game 1" },
                new VideoJuegosEntity { VideojuegoID = 2, Nombre = "Test Game 2" }
            };

        object cacheValue = null;
        _mockCache
            .Setup(c => c.TryGetValue("videojuegosList", out cacheValue))
            .Returns(false); // Simula que no hay datos en la caché

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ListarVideoJuegosQuery>(), default))
            .ReturnsAsync(videojuegos); // Simula que el mediador devuelve datos

        _mockCache
            .Setup(c => c.Set("videojuegosList", videojuegos, It.IsAny<MemoryCacheEntryOptions>())); // Simula que se guarda en la caché

        // Act
        var result = await _service.ListarVideoJuegosService();

        // Assert
        result.Should().BeEquivalentTo(videojuegos);
        _mockCache.Verify(c => c.Set("videojuegosList", videojuegos, It.IsAny<MemoryCacheEntryOptions>()), Times.Once); // Verifica que se guardó en caché
    }



}
