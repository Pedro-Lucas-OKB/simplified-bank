using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.Create;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;       
    }

    public async Task<UserResponse> Handle(CreateUserRequest userRequest, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailOrDocumentAsync(userRequest.Email, userRequest.Document, cancellationToken))
            throw new UserAlreadyExistsException("Já existe um usuário com esse email ou documento.");
        
        var user = User.Create(
            userRequest.FullName,
            userRequest.Email,
            _passwordHasher.Hash(userRequest.Password),
            userRequest.Document,
            userRequest.Type); 
        
        await _userRepository.CreateAsync(user, cancellationToken);;
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return user;
    }
}