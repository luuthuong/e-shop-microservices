namespace Core.Identity;

public sealed record ClientSetting(
    string Id,
    string Secret,
    int AccessTokenLifetime,
    string Scope
);

public sealed record TokenIssuerSettings{
    public required string Authority{get; init;}
    public required ClientSetting UserClient {get; init;}
    public required ClientSetting ApplicationClient {get; init;}
}
