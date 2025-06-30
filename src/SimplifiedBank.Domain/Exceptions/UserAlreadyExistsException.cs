namespace SimplifiedBank.Domain.Exceptions;

public class UserAlreadyExistsException : DomainException
{
    public UserAlreadyExistsException()
    {
        
    }
    
    public UserAlreadyExistsException(string message) 
        : base(message) { }
}