using System.Reflection;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.EventBus;
using Core.Infrastructure.EventBus;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.CQRS;

public static class MediatorExtension
{
    public static IServiceCollection AddCqrs(
        this IServiceCollection services, 
        Action<MediatRServiceConfiguration>? action = null,
        bool enableCache = false
    )
    {
        var assembly = Assembly.GetEntryAssembly()!;
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            action?.Invoke(config);
        });

        services.AddScoped<ICommandBus, CommandBus>();
        services.AddScoped<IQueryBus, QueryBus>();
        
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateRequestBehavior<,>));

        if (enableCache)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRequestBehavior<,>));
        }
            
        return  services;
    }
}