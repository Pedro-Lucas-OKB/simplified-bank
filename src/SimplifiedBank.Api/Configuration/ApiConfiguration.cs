using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace SimplifiedBank.Api.Configuration;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfigurations(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(x =>
            {
                // Evita loops infinitos
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                
                // Ignora propriedades com valores nulos
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        return services;
    }

    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Simplified Bank API",
                Version = "v1",
                Description = "API para sistema bancário simplificado",
                Contact = new OpenApiContact
                {
                    Name = "Pedro Lucas Dev",
                    Email = "pedrolucasep5100@gmail.com",
                    Url = new Uri("https://github.com/Pedro-Lucas-OKB")
                },
                /*License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }*/
            });
        });
        
        return services;
    }
    
    // App
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simplified Bank API v1");
                c.RoutePrefix = string.Empty;
            });
        }
        
        app.MapControllers();
        app.UseHttpsRedirection();
        
        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        
        return app;   
    }
}