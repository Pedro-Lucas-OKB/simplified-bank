using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Transactions.GetAll;

public class GetTransactionsBySenderOrReceiverRequest : IHasGuid, IRequest<List<TransactionResponse>>
{
    public Guid Id { get; }
}