using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Mediator;

public static class MediatorExtensions
{
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return services; 
    }
}