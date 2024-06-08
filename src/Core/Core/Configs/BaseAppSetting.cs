using Core.Identity;
using Core.Redis;

namespace Core.Configs;

public sealed record ConnectionConfig(string Database);

public record BaseAppSettings
{
    public required ConnectionConfig ConnectionStrings { get; init; }
    public required RedisConfig Redis { get; init; }

    public required TokenIssuerSettings TokenIssuerSettings { get; init; }
}