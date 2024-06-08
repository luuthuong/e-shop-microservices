
namespace Core.Identity;

public sealed record TokenIssuerSettings
{
    public required string Authority { get; init; }
    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string Scope { get; init; }
}

public sealed record ClientTokenIssuerSetting
{
    public required string Authority { get; init; }
    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string Scope { get; init; }
}