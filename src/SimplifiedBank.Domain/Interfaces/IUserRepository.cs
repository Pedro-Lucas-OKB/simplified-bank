using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByDocumentAsync(string document, CancellationToken cancellationToken); 
    
    Task<bool> ExistsByEmailOrDocumentAsync(string email, string document, CancellationToken cancellationToken);
}
