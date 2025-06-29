namespace SimplifiedBank.Domain.Exceptions;

public class TransactionNotFoundException : DomainException
{
    public TransactionNotFoundException()
    {
        
    }
    
    public TransactionNotFoundException(string message) 
        : base(message) { }
}