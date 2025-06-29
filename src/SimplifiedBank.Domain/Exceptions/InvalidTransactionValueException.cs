namespace SimplifiedBank.Domain.Exceptions;

public class InvalidTransactionValueException : DomainException
{
    public InvalidTransactionValueException()
    {
        
    }
    
    public InvalidTransactionValueException(string message) 
        : base(message) { }
}