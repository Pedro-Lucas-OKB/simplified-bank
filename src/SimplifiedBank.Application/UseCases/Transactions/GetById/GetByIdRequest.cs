using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Transactions.GetById;

public class GetByIdRequest : IHasGuid, IRequest<TransactionResponse>
{
    public Guid Id { get; }
}