namespace SimplifiedBank.Domain.Exceptions;

public class NoSentTransactionsException : DomainException
{
    public NoSentTransactionsException()
    {
        
    }
    
    public NoSentTransactionsException(string message) 
        : base(message) { }
}