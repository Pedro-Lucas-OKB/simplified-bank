namespace SimplifiedBank.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Value { get; set; }
}
