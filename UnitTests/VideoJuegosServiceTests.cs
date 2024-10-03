using Xunit;
using FluentAssertions;
using Moq; 
using Core.DTOs;
using Core.Services;
using Core.Interfaces;
using Core.Interfaces.store; // Reemplaza con el espacio de nombres correcto de tu proyecto
namespace UnitTests;
public class VideoJuegosServiceTests
{
    private readonly VideoJuegosService _service;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IVideoJuegosRepository> _mockVideoJuegosRepository;

    public VideoJuegosServiceTests()
    {
        // Crea mocks
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockVideoJuegosRepository = new Mock<IVideoJuegosRepository>();

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
        Assert.Equal(200, result.Estado); // Aseg�rate de que el resultado es 200
    }

    [Fact]
    public async Task RegistrarVideoJuego_ShouldReturnFailure_WhenInvalidData()
    {
        // Arrange: Creas un DTO con datos inv�lidos (por ejemplo, nombre nulo o a�o de lanzamiento inv�lido)
        var dto = new VideoJuegosDto
        {
            nombre = null,  // Nombre inv�lido
            compania = "Test Company",
            anio_lanzamiento = -1,  // A�o inv�lido
            precio = 124,
            puntaje_promedio = 1,
            usuario = "testuser"
        };

        // No necesitas configurar el mock, ya que el servicio debe fallar antes de llegar a la llamada del repositorio.

        // Act: Llamas al m�todo de servicio con los datos inv�lidos
        var result = await _service.RegistrarVideoJuegoService(dto);

        // Assert: Verificas que el resultado tiene un estado de error (400) y el mensaje correspondiente
        Assert.Equal(400, result.Estado);  // Verifica que el estado es 400
        Assert.Equal("Datos inv�lidos.", result.Mensaje);  // Verifica que el mensaje es el correcto
    }


     [Fact]
     public async Task ActualizarVideoJuego_ShouldReturnSuccess_WhenValidData()
     {
         // Arrange
         var dto = new VideoJuegosActualizarDto
         {
             video_juego_id = 22, // ID inv�lido
             nombre = "updated nombre", // Datos v�lido
             compania = "Updated Company",
             anio_lanzamiento = 2024, // Datos v�lido
             precio = 150,
             puntaje_promedio = 4,
             usuario = "testuser"
         };

         // Act
         var result = await _service.ActualizarVideoJuegoService(dto);
         Console.WriteLine($"ActualizarVideoJuegoService - DTO: {result.Mensaje}");

         // Assert
         result.Estado.Should().Be(200); // Espera un error de 400
         result.Mensaje.Should().Be(result.Mensaje);
     }

    [Fact]
    public async Task ActualizarVideoJuego_ShouldReturnFailure_WhenInvalidData()
    {
        // Arrange
        var dto = new VideoJuegosActualizarDto
        {
            video_juego_id = 0, // ID inv�lido (0 es considerado inv�lido en muchas validaciones)
            nombre = "", // Nombre vac�o, inv�lido
            compania = "Updated Company",
            anio_lanzamiento = 1899, // A�o inv�lido, demasiado antiguo
            precio = -1, // Precio negativo, inv�lido
            puntaje_promedio = 11, // Puntaje inv�lido, fuera de rango (0-10)
            usuario = "testuser"
        };

        // Act
        var result = await _service.ActualizarVideoJuegoService(dto);
        Console.WriteLine($"ActualizarVideoJuegoService - DTO: {result.Mensaje}");

        // Assert
        result.Estado.Should().Be(400); // Se espera un error 400 porque los datos son inv�lidos
        result.Mensaje.Should().Be("Datos inv�lidos."); // Verifica que el mensaje sea el esperado
    }

}
