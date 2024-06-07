namespace Identity;

public sealed record ClientTokenSetting
{
    public required string Id { get; init; }
    public required string Secret { get; init; }
    public int AccessTokenLifetime { get; init; }
    public required string Scope { get; init; }
}

public sealed class IdentityTokenIssuerSettings
{
    public required string Authority { get; set; }
    public required ClientTokenSetting UserClient { get; set; }
    public required ClientTokenSetting ApplicationClient { get; set; }
}
