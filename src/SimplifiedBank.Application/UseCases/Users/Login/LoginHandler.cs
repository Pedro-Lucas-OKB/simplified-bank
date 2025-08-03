using MediatR;
using SimplifiedBank.Application.Services.Auth;
using SimplifiedBank.Application.Shared.Exceptions;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.Login;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    
    public LoginHandler(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;       
    }
    
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user is null)
            throw new UserNotFoundException("Não foi encontrado um usuário com esse email.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new WrongPasswordException("Senha incorreta.");
        
        var token = _tokenService.GenerateToken(user);

        var response = new LoginResponse
        {
            Token = token,
            IsSuccess = true,
            Message = "Login realizado com sucesso!"
        };
        
        return response;       
    }
}