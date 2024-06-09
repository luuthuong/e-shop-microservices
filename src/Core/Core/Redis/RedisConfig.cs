namespace Core.Redis;

public enum CacheProvider
{
    MemoryCache = 1,
    Redis = 2
}


public sealed record RedisConfig(
    string Host,
    int Port,
    string Password,
    bool Enable,
    CacheProvider? CacheProvider = CacheProvider.MemoryCache
);
