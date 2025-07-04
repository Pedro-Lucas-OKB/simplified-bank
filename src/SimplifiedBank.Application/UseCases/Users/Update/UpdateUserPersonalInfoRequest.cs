using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;

namespace SimplifiedBank.Application.UseCases.Users.Update;

public sealed record UpdateUserPersonalInfoRequest : IHasGuid, IRequest<UserResponse>
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public EUserType Type { get; init; }
}