using FluentValidation;

namespace SimplifiedBank.Application.UseCases.Users.GetAll;

public class GetAllValidator : AbstractValidator<GetAllRequest>
{
    public GetAllValidator()
    {
        RuleFor(request => request.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("O número da página deve ser um inteiro positivo maior ou igual a 1.");
        
        RuleFor(request => request.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("O tamanho da página deve ser um inteiro positivo maior ou igual a 1.");
            
    }
}