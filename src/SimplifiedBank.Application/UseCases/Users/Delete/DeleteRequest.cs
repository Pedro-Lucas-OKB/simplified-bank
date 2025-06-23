using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Users.Delete;

public class DeleteRequest : IHasGuid, IRequest<UserResponse>
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Mapeamento nativo de DeleteRequest para User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator User(DeleteRequest request)
    {
        return new User
        {
            Id = request.Id,
        };
    }
}