using Core.CQRS;
using Core.Infrastructure.CQRS;

namespace API;

public static class Extension
{
    public static IServiceCollection RegisterMediatR(this IServiceCollection service)
    {
        return service.ConfigureMediatR(config =>
        {
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        }, typeof(Exception).Assembly);
    }

    public static IServiceCollection RegisterAutoMapper(this IServiceCollection service)
    {
        return service.AddAutoMapper(typeof(Extension).Assembly);
    }
}