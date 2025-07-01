using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Transactions.Delete;

public sealed record DeleteTransactionRequest : IHasGuid, IRequest<TransactionResponse>
{
    public Guid Id { get; init; }
}