using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Core.Infrastructure.Quartz;

public static class QuartzExtensions
{
    public static IServiceCollection ConfigureQuartz(
        this IServiceCollection services, 
        Action<IServiceCollectionQuartzConfigurator>? action = null)
    {
        services.AddQuartz(config =>
        {
            if (action is not null)
                action(config);
            config.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService();
        return services;
    }
}