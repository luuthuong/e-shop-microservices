namespace Core.Redis;

public sealed record RedisConfig(
    string Host,
    int Port,
    string Password,
    bool Enable
);