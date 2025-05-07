using System.Reflection;
using Core.Configs;
using Core.Domain;
using Core.EF;
using Core.EventBus;
using Core.Http;
using Core.Identity;
using Core.Infrastructure.Api;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.Caching;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF;
using Core.Infrastructure.EF.DBContext;
using Core.Infrastructure.EF.Repository;
using Core.Infrastructure.Http;
using Core.Infrastructure.Identity;
using Core.Infrastructure.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var appSettings = configuration.Get<BaseAppSettings>();

        ArgumentNullException.ThrowIfNull(appSettings);

        services.AddCacheService(appSettings.CachingConfig);

        services.AddCqrs();

        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen();

        services.AddVersioningApi();

        services.AddApiEndpoints(Assembly.GetEntryAssembly()!);

        services.AddSwagger(configuration);

        services.AddJwtAuthentication(appSettings.TokenIssuerSettings);

        services.AddHttpService(configuration);

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IHttpRequest, HttpRequestHandler>();

        return services;
    }

    public static IServiceCollection AddEventSourcing<TAggregateRoot>(this IServiceCollection services, string connectionString) where TAggregateRoot: AggregateRoot, new()
    {
        services.AddDbContext<EventSourcingDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sqlOptions.CommandTimeout(30);
            });
        });

        services.AddScoped<IEventStore<TAggregateRoot>, EventStore<TAggregateRoot>>();
        services.AddSingleton<IEventBus, EventBus.EventBus>();
        services.AddEventHandlers(Assembly.GetEntryAssembly()!);

        return services;
    }

    public static IServiceCollection AddQueryRepository<T, TDbContext>(this IServiceCollection services) where T: class where TDbContext : DbContext 
    {
        services.AddScoped<IQueryRepository<T>>((s) => new QueryRepository<T, TDbContext>(s.GetRequiredService<TDbContext>()));
        return services;
    }

    private static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && 
                          i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        foreach (var handlerType in handlerTypes)
        {
            var eventType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && 
                            i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                .GetGenericArguments()[0];
            
            var handlerInterfaceType = typeof(IEventHandler<>).MakeGenericType(eventType);
            services.AddScoped(handlerInterfaceType, handlerType);
        }

        return services;
    }

    public static BaseAppSettings LoadAppSettings(this IConfiguration configuration)
    {
        var appSettings = configuration.Get<BaseAppSettings>();
        ArgumentNullException.ThrowIfNull(appSettings);
        
        return appSettings;
    }
}