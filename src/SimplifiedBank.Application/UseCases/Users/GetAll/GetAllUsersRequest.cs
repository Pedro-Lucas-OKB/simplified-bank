using MediatR;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Users.GetAll;

public sealed record GetAllUsersRequest : PagedRequest, IRequest<List<UserResponse>>
{
    
}