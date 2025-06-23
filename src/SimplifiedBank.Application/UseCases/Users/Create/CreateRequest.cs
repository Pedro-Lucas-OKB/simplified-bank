using MediatR;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;

namespace SimplifiedBank.Application.UseCases.Users.Create;

public class CreateRequest : IRequest<UserResponse>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public EUserType Type { get; set; }
    
    /// <summary>
    /// Mapeamento nativo de CreateRequest para User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator User(CreateRequest request)
    {
        return new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = request.Password,
            Document = request.Document,
            Type = request.Type
        };   
    }
}