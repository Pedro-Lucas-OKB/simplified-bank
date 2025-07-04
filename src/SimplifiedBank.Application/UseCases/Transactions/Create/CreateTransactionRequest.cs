using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Transactions.Create;

public sealed record CreateTransactionRequest : IRequest<TransactionResponse>
{
    public Guid SenderId { get; init; }
    public Guid ReceiverId { get; init; }
    public decimal Value { get; init; }

    /// <summary>
    /// Mapeamento nativo de CreateTransactionRequest para Transaction
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator Transaction(CreateTransactionRequest request)
        => Transaction.Create(
            request.SenderId,
            request.ReceiverId,
            request.Value);
}