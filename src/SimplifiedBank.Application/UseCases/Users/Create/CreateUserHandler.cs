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
        
        // E-mail de boas-vindas
        try
        {
            await _emailService.SendEmailAsync(
                user.FullName,
                user.Email,
                subject: "Boas-vindas ao Simplified Bank!",
                body: @$"Olá, {user.FullName}! Seja bem-vindo(a) ao Simplified Bank, o seu banco digital simplificado!");
        }
        catch (Exception e)
        {
            new SmtpServerException(e, user.Email, "E-mail de boas-vindas");
        }
        
        return user;
    }
}