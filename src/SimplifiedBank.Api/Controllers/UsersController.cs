using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SimplifiedBank.Application.UseCases.Users.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Api.Dtos.Transactions;
using SimplifiedBank.Api.Dtos.Users;
using SimplifiedBank.Application.Shared;
using SimplifiedBank.Application.Shared.Exceptions;
using SimplifiedBank.Application.UseCases.Transactions.Create;
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

[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [SwaggerOperation(Summary = "Cria um novo usuário (Comum ou Lojista).")]
    [HttpPost("register")]
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

    [AllowAnonymous]
    [SwaggerOperation(Summary = "Realiza o login a partir de um e-mail e uma senha.")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
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
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (WrongPasswordException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error ({e.Message})");
        }
    }

    [SwaggerOperation(Summary = "Atualiza os dados pessoais (nome e e-mail) do usuário atualmente logado (Comum ou Lojista).")]
    [HttpPut("me/update-info")]
    public async Task<IActionResult> UpdateCurrentUserPersonalInfoAsync(
        [FromBody] UpdatePersonalInfoDto updateDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            if (userId == Guid.Empty)
                return Unauthorized("Não foi possível recuperar o ID do usuário. Refaça o login e tente novamente.");

            // Convertendo string para EUserType
            Enum.TryParse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value, out EUserType userType);
            
            var request = new UpdateUserPersonalInfoRequest
            {
                Id = userId,
                FullName = updateDto.FullName,
                Email = updateDto.Email,
                Type = userType
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

    [SwaggerOperation(Summary = "Retorna os dados do usuário atualmente logado (Comum ou Lojista).")]
    [HttpGet("me/get-info")]
    public async Task<IActionResult> GetCurrentUserInfoAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            if (userId == Guid.Empty)
                return Unauthorized("Não foi possível recuperar o ID do usuário. Refaça o login e tente novamente.");
            
            var request = new GetUserByIdRequest
            {
                Id = userId
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

    [Authorize(Roles = "Common, Admin")]
    [SwaggerOperation(Summary = "Retorna as transações enviadas pelo usuário atualmente logado (somente Comum).")]
    [HttpGet("me/get-info/transactions/sent")]
    public async Task<IActionResult> GetCurrentUserSentTransactionsAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();

            if (userId == Guid.Empty)
                return Unauthorized("Não foi possível recuperar o ID do usuário. Refaça o login e tente novamente.");
            
            var request = new GetUserSentTransactionsRequest
            {
                Id = userId
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

    [SwaggerOperation(Summary = "Retorna as transações recebidas pelo usuário atualmente logado (Comum ou Lojista).")]
    [HttpGet("me/get-info/transactions/received")]
    public async Task<IActionResult> GetCurrentUserReceivedTransactionsAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            if (userId == Guid.Empty)
                return Unauthorized("Não foi possível recuperar o ID do usuário. Refaça o login e tente novamente.");
            
            var request = new GetUserReceivedTransactionsRequest
            {
                Id = userId
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
    
    [Authorize(Roles = "Common, Admin")]
    [SwaggerOperation(Summary = "Cria uma nova transação a partir do usuário atualmente logado (somente Comum).")]
    [HttpPost("me/new-transaction")]
    public async Task<IActionResult> SendMoney(
        [FromBody] CreateTransactionForCurrentUserDto createDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var senderId = GetCurrentUserId();

            if (senderId == Guid.Empty)
                return Unauthorized("Não foi possível recuperar o ID do usuário. Refaça o login e tente novamente.");
            
            var request = new CreateTransactionRequest
            {
                SenderId = senderId,
                ReceiverId = createDto.ReceiverId,
                Value = createDto.Value
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
        catch (DomainException e)
        {
            return StatusCode(400, e.Message);
        }
        catch (TransactionNotAuthorizedException e)
        {
            return StatusCode(403, e.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(400, "Alguns dados podem ter sido alterados desde o último carregamento. Tente novamente.");
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
    
    /// <summary>
    /// Função que retorna o ID do usuário atualmente logado
    /// </summary>
    /// <returns></returns>
    private Guid GetCurrentUserId()
    {
        return Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
    }
}