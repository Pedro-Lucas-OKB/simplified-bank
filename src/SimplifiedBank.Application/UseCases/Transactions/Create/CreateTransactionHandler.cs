using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;
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

        if (sender.Type == EUserType.Shopkeeper)
            throw new ShopkeeperCannotTransferException("Usuários lojistas não podem realizar transações, apenas recebê-las.");
        
        // Adicionar serviço validador externo e garantia de unicidade depois
        
        var value = Math.Round(request.Value, 2); // arredondando o valor para 2 casas decimais
        
        //Realizando transação
        sender.Debit(value);
        receiver.Credit(value);
        
        var transaction = Transaction.Create(
            sender.Id, 
            receiver.Id, 
            value);
        await _transactionRepository.CreateAsync(transaction, cancellationToken);
        
        await _userRepository.UpdateAsync(sender, cancellationToken);
        await _userRepository.UpdateAsync(receiver, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);       
        
        return transaction;       
    }
}