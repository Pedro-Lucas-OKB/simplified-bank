using FluentValidation;
using SimplifiedBank.Application.Shared.Validations;
using SimplifiedBank.Domain;

namespace SimplifiedBank.Application.UseCases.Users.Update;

public class UpdatePersonalInfoValidator : BaseEntityValidator<UpdatePersonalInfoRequest>
{
    public UpdatePersonalInfoValidator()
    {
        RuleFor(user => user.FullName)
            .NotEmpty()
            .WithMessage("O nome completo é obrigatório.")
            .MaximumLength(DomainConfiguration.UserFullNameMaximumLength)
            .WithMessage($"O nome deve ter, no máximo, {DomainConfiguration.UserFullNameMaximumLength} caracteres.")
            .MinimumLength(DomainConfiguration.UserFullNameMinimumLength)
            .WithMessage($"O nome deve ter, pelo menos, {DomainConfiguration.UserFullNameMinimumLength} caracteres.")
            .Matches(@"^[a-zA-Z\u00C0-\u017F´]+\s+[a-zA-Z\u00C0-\u017F´]{0,}$")
            .WithMessage("O nome deve ser completo e conter apenas letras e espaços em branco.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("Digite um e-mail válido.");
    }
}