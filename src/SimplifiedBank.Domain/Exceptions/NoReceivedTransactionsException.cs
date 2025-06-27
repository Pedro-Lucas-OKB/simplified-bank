namespace SimplifiedBank.Domain.Exceptions;

public class NoReceivedTransactionsException : Exception
{
    public NoReceivedTransactionsException()
    {
        
    }
    
    public NoReceivedTransactionsException(string message) 
        : base(message) { }   
}