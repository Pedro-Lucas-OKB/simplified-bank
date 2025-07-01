using FluentValidation;
using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Enums;
using SimplifiedBank.Domain.Validators;

namespace SimplifiedBank.Application.UseCases.Users.Create;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
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
            .WithMessage("O nome deve ser completo, cada palavra deve iniciar com letra maiúscula e deve conter apenas letras e espaços em branco.")
            .When(user => user.Type == EUserType.Common);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("Digite um e-mail válido.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.")
            .Matches(DomainConfiguration.UserPasswordRegexPattern)
            .WithMessage(
                "A senha deve conter pelo menos 8 caracteres, com letra maiúscula e minúscula, número e caracteres especiais (@, $, !, %, *, ?, &)")
            .MaximumLength(DomainConfiguration.UserPasswordMaximumLength)
            .WithMessage(
                $"A senha deve conter, no máximo, {DomainConfiguration.UserPasswordMaximumLength} caracteres.");

        RuleFor(user => user.Type)
            .IsInEnum()
            .WithMessage("O tipo de usuário informado não é válido. Indique o tipo de usuário (Comum ou Lojista).");

        RuleFor(user => user.Document)
            .NotEmpty()
            .WithMessage(user => $"O {(user.Type == EUserType.Common ? "CPF" : "CNPJ")} é obrigatório.")
            .Must((user, document) => user.Type == EUserType.Common
                ? CpfValidator.IsValid(document)
                : CnpjValidator.IsValid(document))
            .WithMessage(user => $"O {(user.Type == EUserType.Common ? "CPF" : "CNPJ")} informado é inválido.");
    }
}