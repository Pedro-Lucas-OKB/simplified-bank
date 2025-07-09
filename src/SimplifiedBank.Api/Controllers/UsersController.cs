using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Api.Dtos.Users;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.UseCases.Users.Create;
using SimplifiedBank.Application.UseCases.Users.Delete;
using SimplifiedBank.Application.UseCases.Users.GetAll;
using SimplifiedBank.Application.UseCases.Users.GetById;
using SimplifiedBank.Application.UseCases.Users.GetUserTransactions.Received;
using SimplifiedBank.Application.UseCases.Users.GetUserTransactions.Sent;
using SimplifiedBank.Application.UseCases.Users.Update;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;
using SimplifiedBank.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace SimplifiedBank.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Cria um novo usuário (Comum ou Lojista).")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
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
        catch (DomainException e)
        {
            return StatusCode(400, e.Message);
        }
        catch (DbUpdateException e)
        {
            return StatusCode(400, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error ({e.Message})");
        }
    }

    [SwaggerOperation(Summary = "Atualiza os dados pessoais (nome e e-mail) de um usuário a partir do seu ID.")]
    [HttpPut("update/{type:int:range(0,1)}/{id:guid}")]
    public async Task<IActionResult> UpdatePersonalInfoAsync(
        [FromRoute] int type,
        [FromRoute] Guid id,
        [FromBody] UpdatePersonalInfoDto updateDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new UpdateUserPersonalInfoRequest
            {
                Id = id,
                FullName = updateDto.FullName,
                Email = updateDto.Email,
                Type = (EUserType)type
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
        catch (DbUpdateException e)
        {
            return StatusCode(400, e.Message);
        }
        catch (UserNotFoundException e)
        {
            return StatusCode(404, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error ({e.Message})");
        }
    }

    [SwaggerOperation(Summary = "Exclui um usuário cadastrado a partir do seu ID.")]
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
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

    [SwaggerOperation(Summary = "Retorna os dados de um usuário a partir do seu ID.")]
    [HttpGet("get-info/{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetUserByIdRequest
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
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [SwaggerOperation(Summary = "Retorna uma lista dos usuários cadastrados no sistema.")]
    [HttpGet]
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
    
    [SwaggerOperation(Summary = "Retorna as transações enviadas por um usuário a partir do seu ID.")]
    [HttpGet("get-info/{id:guid}/transactions/sent")]
    public async Task<IActionResult> GetUserSentTransactionsAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetUserSentTransactionsRequest
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
        catch (ShopkeeperCannotTransferException e)
        {
            return StatusCode(400, e.Message);
        }
        catch (NoSentTransactionsException e)
        {
            return StatusCode(404, e.Message);       
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [SwaggerOperation(Summary = "Retorna as transações recebidas por um usuário a partir do seu ID.")]
    [HttpGet("get-info/{id:guid}/transactions/received")]
    public async Task<IActionResult> GetUserReceivedTransactionsAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetUserReceivedTransactionsRequest
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
        catch (NoReceivedTransactionsException e)
        {
            return StatusCode(404, e.Message);       
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}