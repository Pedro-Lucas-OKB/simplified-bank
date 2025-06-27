using MediatR;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Application.UseCases.Users.Create;

public class CreateHandler : IRequestHandler<CreateRequest, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        // gerar hash da senha a ser implementado
        User user = await _userRepository.CreateAsync(request, cancellationToken);;
        
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return user;
    }
}