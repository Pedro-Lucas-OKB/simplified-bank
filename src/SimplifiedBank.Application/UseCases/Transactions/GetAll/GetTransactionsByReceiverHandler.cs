using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.GetAll;

public class GetTransactionsByReceiverHandler : IRequestHandler<GetTransactionsByReceiverRequest, List<TransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public GetTransactionsByReceiverHandler(ITransactionRepository transactionRepository, IUserRepository userRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;   
    }
    
    public async Task<List<TransactionResponse>> Handle(GetTransactionsByReceiverRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (user is null)
            throw new UserNotFoundException("Usuário não pôde ser encontrado.");
        
        var transactions = await _transactionRepository.GetByReceiverIdAsync(request.Id, cancellationToken);

        if (transactions is null)
            throw new NoSentTransactionsException("Não existem transações recebidas por este usuário.");
        
        return TransactionResponse.ConvertAll(transactions);
    }
}