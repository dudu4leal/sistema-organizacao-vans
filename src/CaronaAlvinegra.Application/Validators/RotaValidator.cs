using CaronaAlvinegra.Application.DTOs;
using FluentValidation;

namespace CaronaAlvinegra.Application.Validators;

public class RotaValidator : AbstractValidator<RotaRequest>
{
    public RotaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome da rota é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.LocalEmbarque)
            .NotEmpty().WithMessage("Local de embarque é obrigatório.")
            .MaximumLength(200);
    }
}
