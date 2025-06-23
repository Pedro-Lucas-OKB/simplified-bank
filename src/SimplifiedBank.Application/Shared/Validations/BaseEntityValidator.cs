using FluentValidation;
using SimplifiedBank.Application.Shared.Requests;

namespace SimplifiedBank.Application.Shared.Validations;

public abstract class BaseEntityValidator<T> : AbstractValidator<T> where T : IHasGuid
{
    protected BaseEntityValidator()
    {
        ValidateGuidId();
    }

    private void ValidateGuidId()
    {
        RuleFor(user => user.Id)
            .NotEmpty()
            .WithMessage("O Id não pode estar vazio.")
            .Must(id => id != Guid.Empty)
            .WithMessage("O Id não pode ser um GUID vazio.")
            .Must(BeValidGuid)
            .WithMessage("O Id fornecido não é um GUID válido.");
    }
    
    private bool BeValidGuid(Guid id)
    {
        return Guid.TryParse(id.ToString(), out _);
    }
}