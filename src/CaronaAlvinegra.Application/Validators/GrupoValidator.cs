using CaronaAlvinegra.Application.DTOs;
using FluentValidation;

namespace CaronaAlvinegra.Application.Validators;

public class GrupoValidator : AbstractValidator<GrupoRequest>
{
    public GrupoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome do grupo é obrigatório.")
            .MaximumLength(100);
    }
}
