using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.Create;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionRequest, TransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateTransactionHandler(ITransactionRepository transactionRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;       
    }
    
    public async Task<TransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var sender = await _userRepository.GetByIdAsync(request.SenderId, cancellationToken);
        var receiver = await _userRepository.GetByIdAsync(request.ReceiverId, cancellationToken);

        if (sender is null || receiver is null)
            throw new UserNotFoundException("Não foi possível encontrar o Pagador/Recebedor.");

        var transaction = await _transactionRepository.CreateAsync(request, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);       
        
        return transaction;       
    }
}