using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Users.GetUserTransactions;

public sealed record UserTransactionResponse
{
    public Guid Id { get; init; }
    public decimal Value { get; init; }
    public Guid SenderId { get; init; }
    public string SenderName { get; init; } = string.Empty;
    public Guid ReceiverId { get; init; }
    public string ReceiverName { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    
    /// <summary>
    /// Mapeia valores de Transaction para UserTransactionResponse
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static implicit operator UserTransactionResponse(Transaction transaction)
        => new()
        {
            Id = transaction.Id,
            Value = transaction.Value,
            SenderId = transaction.SenderId,
            SenderName = transaction.Sender.FullName,
            ReceiverId = transaction.ReceiverId,
            ReceiverName = transaction.Receiver.FullName,
            Date = transaction.DateCreated
        };
    
    /// <summary>
    /// Converte uma lista de Transactions em uma lista de UserTransactionResponse
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    public static List<UserTransactionResponse> ConvertAll(IEnumerable<Transaction> transactions)
    {
        List<UserTransactionResponse> transactionResponses = new();

        foreach (var transaction in transactions)
        {
            transactionResponses.Add(transaction);
        }

        return transactionResponses;
    }
}