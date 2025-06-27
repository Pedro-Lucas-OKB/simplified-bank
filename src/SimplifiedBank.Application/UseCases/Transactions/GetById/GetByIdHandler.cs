using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.GetById;

public class GetByIdHandler : IRequestHandler<GetByIdRequest, TransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    
    public GetByIdHandler(ITransactionRepository transactionRepository) 
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<TransactionResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
            throw new TransactionNotFoundException("Não foi encontrada uma transação com o ID informado.");
        
        return transaction;
    }
}