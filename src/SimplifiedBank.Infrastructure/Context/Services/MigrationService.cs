using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimplifiedBank.Application.Services.Database;

namespace SimplifiedBank.Infrastructure.Context.Services;

public class MigrationService : IMigrationService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;       
    }
    
    /// <summary>
    /// Aplica automaticamente, ao inicializar a aplicação, a migração mais recente, caso esteja no ambiente de Desenvolvimento.
    /// </summary>
    public async Task MigrateAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        await scope.ServiceProvider.GetService<ApplicationDbContext>()
            .Database
            .MigrateAsync();
    }
}