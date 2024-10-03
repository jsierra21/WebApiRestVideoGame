using Core.DTOs;
using FluentValidation;

public class VideoJuegosActualizarDtoValidator : AbstractValidator<VideoJuegosActualizarDto>
{
    public VideoJuegosActualizarDtoValidator()
    {
        RuleFor(v => v.nombre)
            .NotEmpty().WithMessage("El nombre del videojuego es obligatorio.")
            .Length(1, 255).WithMessage("El nombre del videojuego debe tener entre 1 y 255 caracteres.");

        RuleFor(v => v.compania)
            .NotEmpty().WithMessage("La compañía es obligatoria.")
            .Length(1, 255).WithMessage("La compañía debe tener entre 1 y 255 caracteres.");

        RuleFor(v => v.anio_lanzamiento)
            .InclusiveBetween(1900, DateTime.Now.Year).WithMessage("El año de lanzamiento debe estar entre 1900 y el año actual.");

        RuleFor(v => v.precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");

        RuleFor(v => v.puntaje_promedio)
            .InclusiveBetween(0, 10).WithMessage("El puntaje promedio debe estar entre 0 y 10.");

        RuleFor(v => v.usuario)
            .NotEmpty().WithMessage("El usuario es obligatorio.")
            .Length(1, 50).WithMessage("El usuario debe tener entre 1 y 50 caracteres.");
    }
}
