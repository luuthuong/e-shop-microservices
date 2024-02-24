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
        });
        services.AddQuartzHostedService();
        return services;
    }

    public static IServiceCollection AddQuartzJob<TJob>(
        this IServiceCollection services) where TJob: IJob

    {
        services.ConfigureQuartz(
            config =>
            {
                var jobKey = new JobKey(nameof(TJob));
                config.AddJob<TJob>(jobKey)
                    .AddTrigger(
                        trigger => trigger
                            .ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule => schedule.WithIntervalInSeconds(5)
                                    .RepeatForever()
                            )
                    );
            }
        );
        return services;
    }
}