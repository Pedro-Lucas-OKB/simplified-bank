using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimplifiedBank.Application.Shared.Behavior;

namespace SimplifiedBank.Application.Services;

public static class ServicesExtensions
{
    public static void AddApplicationConfigurations(this IServiceCollection services)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(ServicesExtensions).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(ServicesExtensions).Assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}