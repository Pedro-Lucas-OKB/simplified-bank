using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Transactions.Delete;

public class DeleteRequest : IHasGuid, IRequest<TransactionResponse>
{
    public Guid Id { get; }
}