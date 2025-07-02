using FluentValidation;
using FluentValidation.Validators;
using SimplifiedBank.Domain;

namespace SimplifiedBank.Application.UseCases.Transactions.Create;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionValidator()
    {
        RuleFor(transaction => new { transaction.SenderId, transaction.ReceiverId })
            .NotEmpty()
            .WithMessage("O ID do Pagador/Recebedor é obrigatório.")
            .Must(t => Guid.TryParse(t.SenderId.ToString(), out _))
            .WithMessage("Insira um ID válido para o pagador.")
            .Must(t => Guid.TryParse(t.ReceiverId.ToString(), out _))
            .WithMessage("Insira um ID válido para o recebedor.");

        RuleFor(transaction => new { transaction.SenderId, transaction.ReceiverId })
            .Must((transaction, ids) => transaction.SenderId != transaction.ReceiverId)
            .WithMessage("O pagador e o recebedor não podem ser iguais.");

        RuleFor(transaction => transaction.Value)
            .NotEmpty()
            .WithMessage("O valor da transferência não pode ser vazio.")
            .GreaterThan(DomainConfiguration.MinTransactionValue)
            .WithMessage($"O valor não pode ser menor que {DomainConfiguration.MinTransactionValue}.")
            .LessThanOrEqualTo(DomainConfiguration.MaxTransactionValue)
            .WithMessage($"O valor da transação não pode ser maior que {DomainConfiguration.MaxTransactionValue}.");
    }
}