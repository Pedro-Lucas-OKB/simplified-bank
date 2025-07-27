namespace SimplifiedBank.Application.Shared.Exceptions;

public class TransactionNotAuthorizedException : Exception
{
    public TransactionNotAuthorizedException(string message) 
        : base(message) { }
}