using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Transactions.Create;

public class CreateRequest : IRequest<TransactionResponse>
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Value { get; set; }

    /// <summary>
    /// Mapeamento nativo de CreateRequest para Transaction
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator Transaction(CreateRequest request)
    {
        return new Transaction
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Value = request.Value,
        };
    }
}