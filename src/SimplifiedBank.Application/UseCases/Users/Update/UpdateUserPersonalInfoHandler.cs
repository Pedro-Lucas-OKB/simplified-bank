using System.ComponentModel.DataAnnotations;
using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.Update;

public class UpdateUserPersonalInfoHandler : IRequestHandler<UpdateUserPersonalInfoRequest, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserPersonalInfoHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResponse> Handle(UpdateUserPersonalInfoRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            throw new UserNotFoundException("Usuário não pôde ser encontrado.");

        if (user.Type != request.Type)
            throw new ValidationException("O tipo informado não condiz com o da base de dados.");
        
        user.UpdatePersonalInfo(request.FullName, request.Email);
        
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return user;       
    }
}