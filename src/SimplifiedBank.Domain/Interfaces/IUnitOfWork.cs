namespace SimplifiedBank.Domain.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}
