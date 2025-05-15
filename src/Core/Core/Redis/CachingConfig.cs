namespace Core.Redis;

public enum CacheProvider
{
    Redis = 1, 
    MemoryCache = 2
}

public sealed record CachingConfig(
    string Host,
    int Port,
    string Password,
    bool Enable,
    CacheProvider? CacheProvider = CacheProvider.MemoryCache
);