namespace SimplifiedBank.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set;}
}
