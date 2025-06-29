namespace SimplifiedBank.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity(Guid id, DateTime dateCreated)
    {
        Id = id;
        DateCreated = dateCreated;
    }

    public Guid Id { get; }
    public DateTime DateCreated { get; }
    public DateTime? DateModified { get; private set;}
    
    protected void UpdateDateModified()
    {
        DateModified = DateTime.UtcNow;
    }
}
