using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Context;

namespace SimplifiedBank.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            return null;
        
        return await Context.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);       
    }

    public async Task<List<TEntity>> GetAllAsync(int skip = 0, int take = 30, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);       
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.AddAsync(entity, cancellationToken);
        return entity;       
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Update(entity);
        return Task.FromResult(entity);
    }

    public Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Remove(entity);
        return Task.FromResult(entity);       
    }
}