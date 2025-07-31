using FluentValidation;
using SimplifiedBank.Domain;

namespace SimplifiedBank.Application.UseCases.Users.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
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
    }
}