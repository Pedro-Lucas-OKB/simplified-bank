using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Transactions.GetAll;

public sealed record GetTransactionsByReceiverRequest : IHasGuid, IRequest<List<TransactionResponse>>
{
    public Guid Id { get; init; }
}