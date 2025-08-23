using System.Net.Mail;
using MediatR;
using SimplifiedBank.Application.Services.Notification;
using SimplifiedBank.Application.Shared.Exceptions;
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
    private readonly IEmailService _emailService;   

    public CreateUserHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;    
        _emailService = emailService;       
    }

    public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailOrDocumentAsync(request.Email, request.Document, cancellationToken))
            throw new UserAlreadyExistsException("Já existe um usuário com esse email ou documento.");
        
        var user = User.Create(
            request.FullName,
            request.Email,
            _passwordHasher.Hash(request.Password),
            request.Document,
            request.Type); 
        
        await _userRepository.CreateAsync(user, cancellationToken);;
        await _unitOfWork.CommitAsync(cancellationToken);
        
        await SendWelcomeEmail(user.Email, user.FullName);       
        
        return user;
    }
    
    // E-mail de boas-vindas para novos usuários
    private async Task SendWelcomeEmail(string email, string fullName)
    {
        try
        {
            await _emailService.SendEmailAsync(
                fullName,
                email,
                subject: "Boas-vindas ao Simplified Bank!",
                body: $@"Olá, {fullName}! <br><br>Seja bem-vindo(a) ao Simplified Bank, o seu banco digital simplificado!");
        }
        catch (Exception e)
        {
            new SmtpServerException(e, email, "E-mail de boas-vindas"); // para logar o erro
        }
    }
}