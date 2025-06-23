using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.GetById;

public class GetByIdHandler : IRequestHandler<GetByIdRequest, UserResponse>
{
    private readonly IUserRepository _userRepository;
    
    public GetByIdHandler(IUserRepository userRepository) 
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException("Usuário não encontrado.");
        }

        return user;
    }
}
