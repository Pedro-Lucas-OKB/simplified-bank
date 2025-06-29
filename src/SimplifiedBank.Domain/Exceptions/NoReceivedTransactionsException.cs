namespace SimplifiedBank.Domain.Exceptions;

public class NoReceivedTransactionsException : DomainException
{
    public NoReceivedTransactionsException()
    {
        
    }
    
    public NoReceivedTransactionsException(string message) 
        : base(message) { }   
}