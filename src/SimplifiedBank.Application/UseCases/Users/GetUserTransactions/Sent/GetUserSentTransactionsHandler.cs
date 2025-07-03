using MediatR;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.GetUserTransactions.Sent;

public class GetUserSentTransactionsHandler : IRequestHandler<GetUserSentTransactionsRequest, GetUserTransactionsResponse>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserSentTransactionsHandler(IUserRepository userRepository) 
    {
        _userRepository = userRepository;
    }
    
    public async Task<GetUserTransactionsResponse> Handle(GetUserSentTransactionsRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithSentTransactions(request.Id, cancellationToken);
        
        if (user is null) 
            throw new UserNotFoundException("Usuário não encontrado.");


        if (!user.TransactionsSent.Any())
            throw new NoSentTransactionsException("Não há transações enviadas.");
            
        return user;       
    }
}