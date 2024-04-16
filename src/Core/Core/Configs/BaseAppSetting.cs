using Core.Redis;

namespace Core.Configs;

public sealed record ConnectionConfig(string Database);

public abstract record BaseAppSettings(
    ConnectionConfig ConnectionStrings,  
    RedisConfig Redis
);