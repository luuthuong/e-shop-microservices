using Core.Identity;
using Core.Redis;

namespace Core.Configs;

public sealed record ConnectionConfig(string Database);

public abstract record BaseAppSettings
{
    public ConnectionConfig ConnectionStrings { get; init; }
    public RedisConfig Redis { get; init; }

    public TokenIssuerSettings TokenIssuerSettings { get; init; }

}