namespace ProductSyncService.Infrastructure.Configs;

public sealed class AppSettings
{
    public ConnectionConfig ConnectionStrings { get; init; }
    public RedisConfig Redis { get; init; }
}