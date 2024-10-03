using Core.DTOs;
using MediatR;

namespace Application.VideoStore.Commands
{
    public class EliminarVideoJuegoCommand : IRequest<ResponseDTO>
    {
        public int VideojuegoID { get; set; } // ID del videojuego a eliminar
    }
}
