using Core.Redis;

namespace ProductSyncService.Infrastructure.Configs;

public sealed record AppSettings(
    ConnectionConfig ConnectionStrings,  
    RedisConfig Redis
);