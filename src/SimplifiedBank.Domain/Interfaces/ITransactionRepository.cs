using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Domain.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<List<Transaction>?> GetBySenderIdAsync(Guid senderId, CancellationToken cancellationToken);
    Task<List<Transaction>?> GetByReceiverIdAsync(Guid receiverId, CancellationToken cancellationToken);
}
