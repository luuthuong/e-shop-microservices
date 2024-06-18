namespace Core.Redis;

public enum CacheProvider
{
    Redis = 1, 
    MemoryCache = 2
}

public sealed record RedisConfig(
    string Host,
    int Port,
    string Password,
    bool Enable,
    CacheProvider? CacheProvider = CacheProvider.MemoryCache
);