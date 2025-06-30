using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Context;
using SimplifiedBank.Infrastructure.Persistence.Repositories;
using SimplifiedBank.Infrastructure.Persistence.UnitOfWork;
using SimplifiedBank.Infrastructure.Security;

namespace SimplifiedBank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPasswordHasher, SecureIdentityPasswordHasher>();
        
        return services;       
    }
}