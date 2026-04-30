using CaronaAlvinegra.Application.DTOs;
using FluentValidation;

namespace CaronaAlvinegra.Application.Validators;

public class JogoValidator : AbstractValidator<JogoRequest>
{
    public JogoValidator()
    {
        RuleFor(x => x.Adversario)
            .NotEmpty().WithMessage("Adversário é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Local)
            .NotEmpty().WithMessage("Local é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.Data)
            .GreaterThan(DateTime.MinValue).WithMessage("Data é obrigatória.");
    }
}
