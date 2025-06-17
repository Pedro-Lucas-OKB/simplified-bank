using SimplifiedBank.Domain.Enums;

namespace SimplifiedBank.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public UserType Type { get; set; }
    public List<Transaction> TransactionsSent { get; set; } = null!;
    public List<Transaction> TransactionsReceived { get; set; } = null!;
}
