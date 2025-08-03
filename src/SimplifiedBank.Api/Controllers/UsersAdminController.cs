using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.UseCases.Users.Delete;
using SimplifiedBank.Application.UseCases.Users.GetAll;
using SimplifiedBank.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace SimplifiedBank.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersAdminController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [SwaggerOperation(Summary = "Exclui um usuário cadastrado a partir do seu ID.")]
    [HttpDelete("delete-user/{id:guid}")]
    public async Task<IActionResult> DeleteUserAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteUserRequest
            {
                Id = id
            };
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Errors.Select(error => new
            {
                Property = error.PropertyName,
                Message = error.ErrorMessage
            }));
        }
        catch (UserNotFoundException e)
        {
            return StatusCode(404, e.Message);
        }
        catch (DbUpdateException e)
        {
            return StatusCode(400, e.Message);
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [SwaggerOperation(Summary = "Retorna uma lista dos usuários cadastrados no sistema.")]
    [HttpGet("get-users")]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = PaginationSettings.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationSettings.DefaultPageSize)
    {
        try
        {
            var request = new GetAllUsersRequest
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            };
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Errors.Select(error => new
            {
                Property = error.PropertyName,
                Message = error.ErrorMessage
            }));
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}