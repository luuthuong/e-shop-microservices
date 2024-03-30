using System.Reflection;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Infrastructure.Utils;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.CQRS;

public static class MediatorExtension
{
    public static IServiceCollection AddCQRS(
        this IServiceCollection services, 
        Action<MediatRServiceConfiguration>? action = null 
    )
    {
        var assemblies = AssemblyUtils.GetAssembliesFromTypes(true, typeof(ICommandHandler<>)).ToList();
        if (!assemblies.Any())
            return services;
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies.ToArray());
            if (action is not null)
                action(config);
        });

        services.AddScoped<ICommandBus, CommandBus>();
        services.AddScoped<IQueryBus, QueryBus>();
        
        services.AddValidatorsFromAssemblies(assemblies.ToArray());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return  services;
    }
}