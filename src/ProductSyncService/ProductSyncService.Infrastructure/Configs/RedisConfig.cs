namespace ProductSyncService.Infrastructure.Configs;

public sealed record RedisConfig(
    string Host,
    int Port,
    string Password
);