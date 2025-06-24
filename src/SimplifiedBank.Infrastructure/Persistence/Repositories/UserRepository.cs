using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Context;

namespace SimplifiedBank.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}