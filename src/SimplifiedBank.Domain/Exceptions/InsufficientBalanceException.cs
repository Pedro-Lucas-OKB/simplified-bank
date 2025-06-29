namespace SimplifiedBank.Domain.Exceptions;

public class InsufficientBalanceException : DomainException
{
    public decimal Balance { get; }
    public InsufficientBalanceException()
    {

    }

    public InsufficientBalanceException(string message)
        : base(message) { }

    public InsufficientBalanceException(string message, decimal balance)
        : this(message)
    { 
        Balance = balance;
    }
}
