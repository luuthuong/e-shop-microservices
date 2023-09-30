using Core.Mediator;
using Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Extensions
{
    public static IServiceCollection RegisterMediatR(this IServiceCollection service)
    {
        return service.ConfigureMediatR(config =>
        {
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        }, typeof(Extensions).Assembly);
    }

    public static IServiceCollection RegisterAutoMapper(this IServiceCollection service)
    {
        return service.AddAutoMapper(typeof(Extensions).Assembly);
    }
}