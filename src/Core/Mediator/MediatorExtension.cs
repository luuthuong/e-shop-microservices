using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Mediator;

public static class MediatorExtension
{
    public static IServiceCollection ConfigureMediatR(
        this IServiceCollection services, 
        Action<MediatRServiceConfiguration>? action = null, 
        params Assembly[] assemblies
        )
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            if (action is not null)
                action(config);
        });
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return services; 
    }
}