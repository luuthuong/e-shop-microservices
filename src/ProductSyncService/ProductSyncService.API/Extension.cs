using AutoMapper;
using Core.CQRS.Command;
using Core.Infrastructure.Utils;
using ProductSyncService.Infrastructure.Persistence;
using MediatorExtension = Core.Infrastructure.CQRS.MediatorExtension;

namespace API;

public static class Extension
{
    public static IServiceCollection GrpcRegister(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        return services;
    }

    public static IServiceCollection RegisterMediatR(this IServiceCollection service)
    {
        var assemblies = AssemblyUtils.GetAssembliesFromTypes(true, typeof(ICommandHandler<>)).ToList();
        return MediatorExtension.ConfigureMediatR(
            service,
            assemblies.ToList(),
            config =>
            {
                config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
    }

    public static IServiceCollection RegisterAutoMapper(this IServiceCollection service)
    {
        var assemblies = typeof(Profile).GetUsedAssemblies(true).ToList();
        return service.AddAutoMapper(assemblies);
    }
}