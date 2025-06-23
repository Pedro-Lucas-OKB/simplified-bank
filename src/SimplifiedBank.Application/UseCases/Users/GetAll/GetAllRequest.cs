using MediatR;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.Shared.Requests;
using SimplifiedBank.Application.Shared.Responses;

namespace SimplifiedBank.Application.UseCases.Users.GetAll;

public sealed record GetAllRequest : PagedRequest, IRequest<List<UserResponse>>
{
    
}