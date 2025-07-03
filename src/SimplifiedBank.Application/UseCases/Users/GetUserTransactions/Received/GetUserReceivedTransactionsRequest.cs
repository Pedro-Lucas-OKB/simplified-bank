using MediatR;
using SimplifiedBank.Application.Shared.Requests;

namespace SimplifiedBank.Application.UseCases.Users.GetUserTransactions.Received;

public sealed record GetUserReceivedTransactionsRequest : IHasGuid, IRequest<GetUserTransactionsResponse>
{
    public Guid Id { get; init; }
}