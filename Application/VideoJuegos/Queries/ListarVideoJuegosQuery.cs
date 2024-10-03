using MediatR;
using Core.Entities;
using System.Collections.Generic;

namespace Application.VideoStore.Queries
{
    public class ListarVideoJuegosQuery : IRequest<List<VideoJuegosEntity>>
    {
    }
}
