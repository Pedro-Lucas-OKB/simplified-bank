namespace SimplifiedBank.Domain.Exceptions;

public class SenderAndReceiverCannotBeEqualException : DomainException
{
    public SenderAndReceiverCannotBeEqualException()
    {
        
    }
    
    public SenderAndReceiverCannotBeEqualException(string message) 
        : base(message) { }   
}