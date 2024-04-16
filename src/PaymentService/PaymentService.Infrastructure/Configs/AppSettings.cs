using Core.Configs;
using Core.Redis;

namespace Infrastructure.Configs;

public sealed record AppSettings(
    ConnectionConfig ConnectionStrings,
    RedisConfig Redis
) : BaseAppSettings(ConnectionStrings, Redis);