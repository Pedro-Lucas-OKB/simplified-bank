using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.GetAll;

public class GetAllHandler : IRequestHandler<GetAllRequest, List<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponse>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            (request.PageNumber - 1) * request.PageSize, 
            request.PageSize, 
            cancellationToken:cancellationToken);

        if (users is null)
            throw new NoUsersOnDatabaseException();
        
        return ConvertAll(users);
    }

    private List<UserResponse> ConvertAll(List<User> users)
    {
        List<UserResponse> userResponses = new();

        foreach (var user in users)
        {
            userResponses.Add(user);
        }
        
        return userResponses;
    }
}