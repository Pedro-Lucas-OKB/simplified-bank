using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Enums;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Transactions.GetAll;

public class GetTransactionsBySenderHandler : IRequestHandler<GetTransactionsBySenderRequest, List<TransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public GetTransactionsBySenderHandler(ITransactionRepository transactionRepository, IUserRepository userRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;   
    }

    public async Task<List<TransactionResponse>> Handle(GetTransactionsBySenderRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (user is null)
            throw new UserNotFoundException("Usuário não pôde ser encontrado.");
        
        if (user.Type == EUserType.Shopkeeper)
            throw new ShopkeeperCannotTransferException("Apenas usuários comuns podem enviar transferências.");
        
        var transactions = await _transactionRepository.GetBySenderIdAsync(request.Id, cancellationToken);

        if (!transactions.Any())
            throw new NoSentTransactionsException("Não existem transações feitas por este usuário.");
        
        return TransactionResponse.ConvertAll(transactions);
    }
}