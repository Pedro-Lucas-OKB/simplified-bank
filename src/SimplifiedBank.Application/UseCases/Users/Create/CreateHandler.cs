using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.Create;

public class CreateHandler : IRequestHandler<CreateRequest, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;       
    }

    public async Task<UserResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.FullName,
            request.Email,
            _passwordHasher.Hash(request.Password),
            request.Document,
            request.Type); 
        
        await _userRepository.CreateAsync(user, cancellationToken);;
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return user;
    }
}