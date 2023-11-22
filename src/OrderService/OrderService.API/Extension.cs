using AutoMapper;
using Core.CQRS.Command;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.Utils;

namespace API;

public static class Extension
{
    public static IServiceCollection AddCQRS(this IServiceCollection service)
    {
        var assemblies = typeof(ICommandHandler<>).GetUsedAssemblies(true).ToList();
        assemblies.Add(typeof(Extension).Assembly);
        return MediatorExtension.ConfigureMediatR(
            service,
            assemblies.ToList(),
            config =>
            {
                config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
    }
    
    public static IServiceCollection AddAutoMapper(this IServiceCollection service)
    {
        var assemblies = typeof(Profile).GetUsedAssemblies(true).ToList();
        return service.AddAutoMapper(assemblies);
    }
}