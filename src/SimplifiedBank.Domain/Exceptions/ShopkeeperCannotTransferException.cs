namespace SimplifiedBank.Domain.Exceptions;

public class ShopkeeperCannotTransferException : DomainException
{
    public ShopkeeperCannotTransferException()
    {

    }

    public ShopkeeperCannotTransferException(string message)
        : base(message) { }
}
