using MediatR;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.GetUserTransactions.Received;

public class GetUserReceivedTransactionsHandler : IRequestHandler<GetUserReceivedTransactionsRequest, GetUserTransactionsResponse>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserReceivedTransactionsHandler(IUserRepository userRepository) 
    {
        _userRepository = userRepository;
    }
    
    public async Task<GetUserTransactionsResponse> Handle(GetUserReceivedTransactionsRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithReceivedTransactions(request.Id, cancellationToken);
        
        if (user is null) 
            throw new UserNotFoundException("Usuário não encontrado.");


        if (!user.TransactionsReceived.Any())
            throw new NoReceivedTransactionsException("Não há transações recebidas.");
            
        return user;
    }
}