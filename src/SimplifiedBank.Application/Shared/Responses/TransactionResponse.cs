using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.Shared.Responses;

public sealed record TransactionResponse
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Value { get; set; }
    public DateTime Date { get; set; }

    /// <summary>
    /// Mapeamento nativo de Transaction para TransactionResponse
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static implicit operator TransactionResponse(Transaction transaction)
        => new()
        {
            Id = transaction.Id,
            SenderId = transaction.SenderId,
            ReceiverId = transaction.ReceiverId,
            Value = transaction.Value,
            Date = transaction.DateCreated,
        };

    /// <summary>
    /// Converte uma lista de Transactions em uma lista de TransactionResponse
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    public static List<TransactionResponse> ConvertAll(IEnumerable<Transaction> transactions)
    {
        List<TransactionResponse> transactionResponses = new();

        foreach (var transaction in transactions)
        {
            transactionResponses.Add(transaction);
        }

        return transactionResponses;
    }
}