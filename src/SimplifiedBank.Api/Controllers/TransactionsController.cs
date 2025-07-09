using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Application.UseCases.Transactions.Create;
using SimplifiedBank.Application.UseCases.Transactions.Delete;
using SimplifiedBank.Application.UseCases.Transactions.GetAll;
using SimplifiedBank.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace SimplifiedBank.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Cria uma nova transação.")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateTransactionRequest request,
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
    
    // Possíveis mudanças na lógica de exclusão no futuro
    [SwaggerOperation(Summary = "Exclui uma transação existente a partir do seu ID.")]
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteTransactionRequest
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
        catch (TransactionNotFoundException e)
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

    [SwaggerOperation(Summary = "Retorna uma lista de transações enviadas por um usuário a partir de seu ID.")]
    [HttpGet("get-by-sender/{senderId:guid}")]
    public async Task<IActionResult> GetBySenderAsync(
        [FromRoute] Guid senderId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTransactionsBySenderRequest
            {
                Id = senderId
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
    
    [SwaggerOperation(Summary = "Retorna uma lista de transações recebidas por um usuário a partir de seu ID.")]
    [HttpGet("get-by-receiver/{receiverId:guid}")]
    public async Task<IActionResult> GetByReceiverAsync(
        [FromRoute] Guid receiverId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTransactionsByReceiverRequest
            {
                Id = receiverId
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