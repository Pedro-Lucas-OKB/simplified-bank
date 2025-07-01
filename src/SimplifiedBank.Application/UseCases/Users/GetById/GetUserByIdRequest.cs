using MediatR;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Users.GetById;

public sealed record GetUserByIdRequest : IHasGuid, IRequest<UserResponse>
{
    public Guid Id { get; init; }
}
