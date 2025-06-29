namespace SimplifiedBank.Domain.Exceptions;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException()
    {

    }
    public UserNotFoundException(string message) 
        : base(message) { }
}
