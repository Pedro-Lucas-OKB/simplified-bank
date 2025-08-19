namespace SimplifiedBank.Application.Services.Database;

public interface IMigrationService
{
    Task MigrateAsync();   
}