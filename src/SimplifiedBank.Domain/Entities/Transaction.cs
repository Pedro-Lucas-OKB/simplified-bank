namespace SimplifiedBank.Domain.Entities;

public class Transaction : BaseEntity
{
    public Transaction() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
    public decimal Value { get; set; }
}
