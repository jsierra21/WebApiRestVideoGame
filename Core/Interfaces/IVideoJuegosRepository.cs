﻿

using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.store
{
    public interface IVideoJuegosRepository
    {
        Task<VideoJuegosEntity> RegistrarVideoJuego(VideoJuegosDto video); // Acepta un objeto UsuarioEntity como parámetro
        Task<List<VideoJuegosEntity>> ListarVideoJuegos();


    }
}
