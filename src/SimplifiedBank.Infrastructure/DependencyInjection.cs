using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplifiedBank.Application.Services.Auth;
using SimplifiedBank.Application.Services.TransactionAuthorization;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Infrastructure.Context;
using SimplifiedBank.Infrastructure.Persistence.Repositories;
using SimplifiedBank.Infrastructure.Persistence.UnitOfWork;
using SimplifiedBank.Infrastructure.Security;
using SimplifiedBank.Infrastructure.Security.Auth;
using SimplifiedBank.Infrastructure.Security.TransactionAuthorization;

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
        
        services.AddHttpClient<ITransactionAuthorizerService, TransactionAuthorizerService>();

        services.AddScoped<IPasswordHasher, SecureIdentityPasswordHasher>();

        services.AddTransient<ITokenService, TokenService>();
        
        return services;       
    }
}