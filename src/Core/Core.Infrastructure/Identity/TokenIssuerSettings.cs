namespace Core.Infrastructure.Identity;

public record TokenIssuerSettings
{
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Scope { get; set; }
}