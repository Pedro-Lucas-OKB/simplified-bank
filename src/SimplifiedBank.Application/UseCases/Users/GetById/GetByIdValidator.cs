using FluentValidation;

namespace SimplifiedBank.Application.UseCases.Users.GetById;

public class GetByIdValidator : AbstractValidator<GetByIdRequest>
{
    public GetByIdValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty()
            .WithMessage("O Id não pode estar vazio")
            .Must(id => id != Guid.Empty)
            .WithMessage("O Id não pode ser um GUID vazio")
            .Must(BeValidGuid)
            .WithMessage("O Id fornecido não é um GUID válido");
    }
    
    private bool BeValidGuid(Guid id)
    {
        return Guid.TryParse(id.ToString(), out _);
    }
}