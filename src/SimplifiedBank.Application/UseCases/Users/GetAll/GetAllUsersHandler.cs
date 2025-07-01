using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.GetAll;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, List<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponse>> Handle(GetAllUsersRequest usersRequest, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            (usersRequest.PageNumber - 1) * usersRequest.PageSize, 
            usersRequest.PageSize, 
            cancellationToken:cancellationToken);

        if (users is null)
            throw new NoUsersOnDatabaseException("Não há usuários cadastrados.");
        
        return UserResponse.ConvertAll(users);
    }
}