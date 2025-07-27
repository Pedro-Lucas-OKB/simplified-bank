namespace SimplifiedBank.Application.Services.TransactionAuthorization;

public interface ITransactionAuthorizerService
{
    Task<bool> IsAuthorizedAsync();
}