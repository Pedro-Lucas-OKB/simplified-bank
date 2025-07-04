using MediatR;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;

namespace SimplifiedBank.Application.UseCases.Users.Create;

public sealed record CreateUserRequest : IRequest<UserResponse>
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Document { get; init; } = string.Empty;
    public EUserType Type { get; init; }

    /// <summary>
    /// Mapeamento nativo de CreateTransactionRequest para User
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    public static implicit operator User(CreateUserRequest userRequest)
        => User.Create(
            userRequest.FullName,
            userRequest.Email,
            userRequest.Password,
            userRequest.Document,
            userRequest.Type);
}