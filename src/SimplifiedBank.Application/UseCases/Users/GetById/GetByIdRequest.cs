using MediatR;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Users.GetById;

public sealed record GetByIdRequest : IRequest<GetByIdResponse>
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Mapeamento nativo de GetByIdRequest para User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator User(GetByIdRequest request)
    {
        return new User
        {
            Id = request.Id,
        };
    }
}
