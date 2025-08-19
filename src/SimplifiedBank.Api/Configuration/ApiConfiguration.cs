using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimplifiedBank.Application.Services.Database;
using SimplifiedBank.Infrastructure;
using SimplifiedBank.Infrastructure.Context.Services;

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

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer", 
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }

    // App
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            _ = app.ApplyDatabaseMigrations();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simplified Bank API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        InfrastructureConfiguration.JwtKey = builder.Configuration.GetValue<string>("JwtKey") ?? string.Empty;
        var key = Encoding.ASCII.GetBytes(InfrastructureConfiguration.JwtKey);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
    }
    
    public static async Task<IApplicationBuilder> ApplyDatabaseMigrations(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var migrationService = scope.ServiceProvider
            .GetRequiredService<IMigrationService>();
        await migrationService.MigrateAsync();
        return app;
    }

}