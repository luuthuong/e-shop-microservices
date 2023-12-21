using AutoMapper;
using Core.CQRS.Command;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.Quartz;
using Core.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using ProductSyncService.Infrastructure.Configs;
using ProductSyncService.Infrastructure.Outbox;
using Quartz;
using StackExchange.Redis;
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

    public static IServiceCollection AddMediatR(this IServiceCollection service)
    {
        var assemblies = AssemblyUtils.GetAssembliesFromTypes(true, typeof(ICommandHandler<>)).ToList();
        return MediatorExtension.ConfigureMediatR(
            service,
            assemblies.ToList(),
            config =>
            {
                config.AddOpenBehavior(typeof(ProductSyncService.Infrastructure.Persistence.UnitOfWorkBehavior<,>));
                config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
            });
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection service)
    {
        var assemblies = typeof(Profile).GetUsedAssemblies(true).ToList();
        return service.AddAutoMapper(assemblies);
    }

    public static IServiceCollection AddRedis(this IServiceCollection service)
    {
        var appSettings = service.BuildServiceProvider().GetService<IOptions<AppSettings>>();
        var (host, port, password) = appSettings!.Value.Redis;
        
        service.AddStackExchangeRedisCache(redisOption =>
        {
            redisOption.ConfigurationOptions = new ConfigurationOptions()
            {
                EndPoints = {$"{host}:{port}"},
                Password = password
            };
        });
        return service;
    }

    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.ConfigureQuartz(
            config =>
            {
                var jobKey = new JobKey(nameof(OutBoxMessageJob));
                config.AddJob<OutBoxMessageJob>(jobKey)
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