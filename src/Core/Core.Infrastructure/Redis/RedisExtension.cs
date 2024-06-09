using Core.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Infrastructure.Redis;

public static class RedisExtension
{
    public static IServiceCollection AddCacheService(this IServiceCollection services, RedisConfig config)
    {
        var (host, port, password, enable, cacheProvider) = config;
        
        if (!enable)
            return services;

        var cacheServiceImpl = cacheProvider switch
        {
            CacheProvider.Redis => services.ConfigureRedis(config),
            CacheProvider.MemoryCache => services.ConfigureMemoryCache(),
            _ => services.ConfigureMemoryCache()
        };

        services.AddScoped(typeof(ICacheService), cacheServiceImpl);
        
        return services;
    }

    private static Type ConfigureRedis(this IServiceCollection services, RedisConfig config)
    {
        services.AddStackExchangeRedisCache(redisOption =>
        {
            redisOption.ConfigurationOptions = new ConfigurationOptions()
            {
                EndPoints = {$"{config.Host}:{config.Port}"},
                Password = config.Password
            };
        });
        return typeof(RedisCacheService);
    }

    private static Type ConfigureMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return typeof(MemoryCacheService);
    }
}