using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Context;

namespace SimplifiedBank.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<User?> GetByDocumentAsync(string document, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(document))
            return null;

        return await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Document == document, cancellationToken);
    }

    public async Task<bool> ExistsByEmailOrDocumentAsync(string email, string document,
        CancellationToken cancellationToken)
    {
        email = email.Trim();
        document = User.NormalizeDocument(document);

        return await Context.Users.AnyAsync(x => x.Email == email || x.Document == document, cancellationToken);
    }

    public async Task<User?> GetUserWithSentTransactions(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Users
            .AsNoTracking()
            .Include(u
                => u.TransactionsSent
                    .OrderByDescending(t => t.DateCreated))
            .ThenInclude(t => t.Receiver)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task<User?> GetUserWithReceivedTransactions(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Users
            .AsNoTracking()
            .Include(u 
                => u.TransactionsReceived
                    .OrderByDescending(t => t.DateCreated))
            .ThenInclude(t => t.Sender)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}