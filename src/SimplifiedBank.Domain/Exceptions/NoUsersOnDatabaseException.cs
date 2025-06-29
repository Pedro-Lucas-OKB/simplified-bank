namespace SimplifiedBank.Domain.Exceptions;

public class NoUsersOnDatabaseException : DomainException
{
    public NoUsersOnDatabaseException()
    {
    }

    public NoUsersOnDatabaseException(string message) 
        : base(message)
    {
    }
}