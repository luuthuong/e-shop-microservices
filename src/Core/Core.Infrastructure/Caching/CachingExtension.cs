﻿using Core.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Infrastructure.Caching;

public static class CachingExtension
{
    public static IServiceCollection AddCacheService(this IServiceCollection services, CachingConfig config)
    {
        var cacheServiceImpl = config.CacheProvider switch
        {
            CacheProvider.Redis =>  services.ConfigureRedis(config),
            CacheProvider.MemoryCache =>  services.ConfigureMemoryCache(),
            _ => typeof(NonCachingService)
        };
        
        services.AddScoped(typeof(ICacheService), cacheServiceImpl);
        return services;
    }

    private static Type ConfigureMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return typeof(MemoryCacheService);
    }

    private static Type ConfigureRedis(this IServiceCollection services, CachingConfig cachingConfig)
    {
        services.AddStackExchangeRedisCache(redis =>
        {
            redis.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{cachingConfig.Host}:{cachingConfig.Port}" },
                Password = cachingConfig.Password
            };
        });
        return typeof(RedisCacheService);
    }
}