using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.Delete;

public class DeleteHandler : IRequestHandler<DeleteRequest, TransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionResponse> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction is null)
            throw new TransactionNotFoundException("Não foi possível encontrar uma transação com este id.");
        
        await _transactionRepository.DeleteAsync(transaction, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return transaction;       
    }
}