namespace SimplifiedBank.Infrastructure.Security.TransactionAuthorization;

public class TransactionAuthorizerOut
{
    public string Status { get; set; } = string.Empty;
    public TransactionAuthorizerDataOut Data { get; set; } = null!;
}

public class TransactionAuthorizerDataOut
{
    public bool Authorization { get; set; }
}