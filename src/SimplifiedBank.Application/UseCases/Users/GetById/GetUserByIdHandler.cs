using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.GetById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, UserResponse>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserByIdHandler(IUserRepository userRepository) 
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException("Usuário não encontrado.");
        }

        return user;
    }
}
