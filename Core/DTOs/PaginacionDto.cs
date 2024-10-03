using Newtonsoft.Json;

namespace Core.DTOs
{
    public class PaginacionDto
    {
        public int Pagina { get; set; } = 1; // Número de página (default: 1)
        public int RegistrosPorPagina { get; set; } = 5; // Registros por página (default: 5)
    }

}
