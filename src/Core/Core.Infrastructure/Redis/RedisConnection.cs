using StackExchange.Redis;

namespace Core.Infrastructure.Redis;

public class RedisConnection
{
    private static Lazy<ConnectionMultiplexer> _lazy;
    public static ConnectionMultiplexer Connection => _lazy.Value;

    static RedisConnection()
    {
        _lazy = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(""));
    }
}