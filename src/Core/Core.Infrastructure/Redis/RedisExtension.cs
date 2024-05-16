using Core.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Infrastructure.Redis;

public static class RedisExtension
{
    public static IServiceCollection AddRedis(this IServiceCollection services, RedisConfig config)
    {
        var (host, port, password, enable) = config;
        if (!enable)
            return services;
            
        services.AddStackExchangeRedisCache(redisOption =>
        {
            redisOption.ConfigurationOptions = new ConfigurationOptions()
            {
                EndPoints = {$"{host}:{port}"},
                Password = password
            };
        });
        return services;
    }
}