using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.GetById;

public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdRequest, TransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    
    public GetTransactionByIdHandler(ITransactionRepository transactionRepository) 
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<TransactionResponse> Handle(GetTransactionByIdRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
            throw new TransactionNotFoundException("Não foi encontrada uma transação com o ID informado.");
        
        return transaction;
    }
}