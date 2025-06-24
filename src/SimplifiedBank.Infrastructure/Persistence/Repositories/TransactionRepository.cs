using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Context;

namespace SimplifiedBank.Infrastructure.Persistence.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Transaction>?> GetBySenderIdAsync(Guid senderId, CancellationToken cancellationToken)
    {
        return await Context.Transactions
            .AsNoTracking()
            .Where(x => x.SenderId == senderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Transaction>?> GetByReceiverIdAsync(Guid receiverId, CancellationToken cancellationToken)
    {
        return await Context.Transactions
            .AsNoTracking()
            .Where(x => x.ReceiverId == receiverId)
            .ToListAsync(cancellationToken);
    }
}