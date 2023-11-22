using System.Reflection;
using Core.CQRS.Command;
using Core.CQRS.Query;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.CQRS;

public static class MediatorExtension
{
    public static IServiceCollection ConfigureMediatR(
        this IServiceCollection services, 
        Action<MediatRServiceConfiguration>? action = null, 
        params Assembly[] assemblies
        ) => services.Register(assemblies, action);

    public static IServiceCollection ConfigureMediatR(
        this IServiceCollection services, 
        IList<Assembly> assemblies,
        Action<MediatRServiceConfiguration>? action = null 
    ) => services.Register(assemblies, action);

    private static IServiceCollection Register(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        Action<MediatRServiceConfiguration>? action = null
        )
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies.ToArray());
            if (action is not null)
                action(config);
        });

        services.AddScoped<ICommandBus, CommandBus>();
        services.AddScoped<IQueryBus, QueryBus>();
        
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return  services;
    }
}