using MediatR;
using SimplifiedBank.Application.Shared.Requests;

namespace SimplifiedBank.Application.UseCases.Users.GetUserTransactions.Sent;

public sealed record GetUserSentTransactionsRequest : IHasGuid, IRequest<GetUserTransactionsResponse>
{
    public Guid Id { get; init; }
}