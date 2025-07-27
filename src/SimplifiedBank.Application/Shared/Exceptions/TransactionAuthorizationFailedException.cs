namespace SimplifiedBank.Application.Shared.Exceptions;

public class TransactionAuthorizationFailedException : Exception
{
    public TransactionAuthorizationFailedException(string message) 
        : base(message) { }
}