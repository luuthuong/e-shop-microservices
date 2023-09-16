using System.Reflection;
using Application.CQRS.Behavior;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Mediator;

public static class MediatorExtensions
{
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services, Assembly? assembly)
    {
        services.AddMediatR(config =>
        {
            if (assembly != null) config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        return services; 
    }
    
}