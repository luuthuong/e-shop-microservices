using System.Reflection;
using Asp.Versioning;
using Core.Configs;
using Core.Infrastructure.Api;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF;
using Core.Infrastructure.EF.DbContext;
using Core.Infrastructure.Outbox.Worker;
using Core.Infrastructure.Quartz;
using Core.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Core.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddCoreInfrastructure<TDbContext>(this IServiceCollection services,
        BaseAppSettings appSettings) where TDbContext : BaseDbContext
    {
        services.AddAppDbContext<TDbContext>(
            config =>
            {
                var database = appSettings.ConnectionStrings.Database;
                if (string.IsNullOrEmpty(database))
                    throw new ArgumentNullException();

                Console.WriteLine($"Connection String: {database}");
                return config.UseSqlServer(database,
                    sqlConfig => { sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null); });
            }
        );
        
        services.AddRedis(appSettings.Redis);
        
        services.AddRepositories(appSettings.Redis.Enable);
        
        services.AddCQRS(
            config => { config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>)); }
        );
        
        services.AddAutoMapper();
        
        services.AddQuartzJob<OutBoxMessageJob<TDbContext>>();
        
        services.AddHttpContextAccessor();

        services.AddSwaggerGen(
            option => option.EnableAnnotations()
        );
        
        services.AddApiEndpoints(Assembly.GetCallingAssembly());
        
        services.AddEndpointsApiExplorer();

        services.AddVersioningApi();
        
        return services;
    }
}