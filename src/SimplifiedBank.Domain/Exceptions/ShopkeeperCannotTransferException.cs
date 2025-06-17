namespace SimplifiedBank.Domain.Exceptions;

public class ShopkeeperCannotTransferException : Exception
{
    public ShopkeeperCannotTransferException()
    {

    }

    public ShopkeeperCannotTransferException(string message)
        : base(message) { }
}
