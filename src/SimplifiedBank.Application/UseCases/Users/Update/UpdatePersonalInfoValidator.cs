using FluentValidation;
using SimplifiedBank.Application.Shared.Validations;
using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Enums;

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
            .WithMessage($"O nome deve ter, pelo menos, {DomainConfiguration.UserFullNameMinimumLength} caracteres.");
            
        RuleFor(user => user.FullName)
            .Matches(DomainConfiguration.CommonUserFullNameRegexPattern)
            .WithMessage("O nome deve ser completo, cada palavra deve iniciar com letra maiúscula e deve conter apenas letras e espaços em branco. Caso esteja querendo alterar um lojista, selecione o tipo correto.")
            .When(user => user.Type == EUserType.Common);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("Digite um e-mail válido.");
        
        RuleFor(user => user.Type)
            .IsInEnum()
            .WithMessage("O tipo de usuário informado não é válido. Indique o tipo de usuário (Comum ou Lojista).");
    }
}