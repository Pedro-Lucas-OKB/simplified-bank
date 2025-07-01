using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Transactions.Create;

public sealed record CreateTransactionRequest : IRequest<TransactionResponse>
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Value { get; set; }

    /// <summary>
    /// Mapeamento nativo de CreateTransactionRequest para Transaction
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator Transaction(CreateTransactionRequest request)
    {
        return Transaction.Create(
            request.SenderId,
            request.ReceiverId,
            request.Value);
    }
}