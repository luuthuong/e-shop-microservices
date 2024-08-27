using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Core.Infrastructure.Quartz;

public static class QuartzExtensions
{
    public static IServiceCollection AddQuartzJob<TJob>( this IServiceCollection services) where TJob: IJob
    {
        services.AddQuartz(
                config =>
                {
                    var jobKey = new JobKey(nameof(TJob));
                    config.AddJob<TJob>(opt => opt.WithIdentity(jobKey))
                        .AddTrigger(
                            trigger => trigger
                                .ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule => schedule.WithIntervalInSeconds(5)
                                        .RepeatForever()
                                )
                        );
                }
            )
            .AddQuartzHostedService(
                config =>
                {
                    config.WaitForJobsToComplete = true;
                }
            );
        
        return services;
    }
}