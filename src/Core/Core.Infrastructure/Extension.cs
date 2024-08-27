using System.Reflection;
using Core.Configs;
using Core.Http;
using Core.Identity;
using Core.Infrastructure.Api;
using Core.Infrastructure.AutoMapper;
using Core.Infrastructure.Caching;
using Core.Infrastructure.CQRS;
using Core.Infrastructure.EF;
using Core.Infrastructure.EF.DbContext;
using Core.Infrastructure.Http;
using Core.Infrastructure.Identity;
using Core.Infrastructure.Outbox.Worker;
using Core.Infrastructure.Quartz;
using Core.Infrastructure.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddCoreInfrastructure<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, string appSettingSection = "") where TDbContext : BaseDbContext
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        services.ConfigureSerilog(configuration);

        var appSettings = string.IsNullOrEmpty(appSettingSection)
            ? configuration.Get<BaseAppSettings>()
            : configuration.GetSection(appSettingSection).Get<BaseAppSettings>();

        if (appSettings is null)
            throw new ArgumentNullException(nameof(appSettings));

        services.AddAppDbContext<TDbContext>(
            config =>
            {
                var database = appSettings.ConnectionStrings.Database;
                if (string.IsNullOrEmpty(database))
                    throw new ArgumentNullException();

                Log.Information($"Connection String: {database}");
                return config.UseSqlServer(database,
                    sqlConfig => { sqlConfig.EnableRetryOnFailure(5, TimeSpan.FromSeconds(15), null); });
            }
        );

        services.AddCacheService(appSettings.Redis);
        services.AddRepositories(appSettings.Redis.Enable);
        services.AddCQRS(
            config => { config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>)); }
        );

        services.AddAutoMapper();

        services.AddQuartzJob<OutBoxMessageJob<TDbContext>>();

        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        services.AddVersioningApi();

        services.AddApiEndpoints(Assembly.GetCallingAssembly());

        services.AddSwagger(configuration);

        services.AddJwtAuthentication(appSettings.TokenIssuerSettings);

        services.AddHttpClient();

        services.AddMemoryCache();


        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IHttpRequest, HttpRequestHandler>();

        return services;
    }
}
