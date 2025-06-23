using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Users.Update;

public class UpdatePersonalInfoRequest : IHasGuid, IRequest<UserResponse>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Mapeamento nativo de UpdatePersonalInfoRequest para User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator User(UpdatePersonalInfoRequest request)
    {
        return new User
        {
            Id = request.Id,
            FullName = request.FullName,
            Email = request.Email
        };
    }
}