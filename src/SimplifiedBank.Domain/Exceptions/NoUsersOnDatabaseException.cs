namespace SimplifiedBank.Domain.Exceptions;

public class NoUsersOnDatabaseException : Exception
{
    public NoUsersOnDatabaseException()
    {
    }

    public NoUsersOnDatabaseException(string message) 
        : base(message)
    {
    }
}