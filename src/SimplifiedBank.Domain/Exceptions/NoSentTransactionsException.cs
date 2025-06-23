namespace SimplifiedBank.Domain.Exceptions;

public class NoSentTransactionsException : Exception
{
    public NoSentTransactionsException()
    {
        
    }
    
    public NoSentTransactionsException(string message) 
        : base(message) { }
}