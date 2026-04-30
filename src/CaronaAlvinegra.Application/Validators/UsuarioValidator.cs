using CaronaAlvinegra.Application.DTOs;
using FluentValidation;

namespace CaronaAlvinegra.Application.Validators;

public class UsuarioValidator : AbstractValidator<UsuarioRequest>
{
    public UsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.RotaPreferencialId)
            .NotEmpty().WithMessage("Rota preferencial é obrigatória.");
    }
}

public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioRequest>
{
    public UpdateUsuarioValidator()
    {
        When(x => x.Nome != null, () =>
        {
            RuleFor(x => x.Nome!)
                .NotEmpty().WithMessage("Nome não pode ser vazio.")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");
        });
    }
}
