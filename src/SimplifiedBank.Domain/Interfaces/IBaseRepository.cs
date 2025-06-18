using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<List<T>?> GetAllAsync(int skip = 0, int take = 30, CancellationToken cancellationToken = default);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> Delete(T entity, CancellationToken cancellationToken = default);
}
