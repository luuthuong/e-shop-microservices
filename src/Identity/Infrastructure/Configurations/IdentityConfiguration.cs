using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.Infrastructure.Configurations;

public abstract class IdentityConfiguration
{
    private const string ApiScope = "eshop-api.scope";
    private const string ReadScope = "read";
    private const string WriteScope = "write";
    private const string DeleteScope = "delete";

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("eshop-api")
            {
                Scopes = new List<string>
                {
                    ApiScope,
                    ReadScope,
                    WriteScope,
                    DeleteScope
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new(name: ApiScope, displayName: "Eshop api"),
            new(name: ReadScope, displayName: "Read your data."),
            new(name: WriteScope, displayName: "Write your data."),
            new(name: DeleteScope, displayName: "Delete your data."),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // User's client
            new Client
            {
                ClientId = "eshop.user_client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true,
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret234554^&%&^%&^f2%%%".Sha256())
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    ReadScope,
                    WriteScope,
                    DeleteScope
                },
                AccessTokenLifetime = 86400
            },
            // machine to machine client
            new Client
            {
                ClientId = "eshop.application_client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RequireClientSecret = true,
                ClientSecrets = new List<Secret> { new("secret33587^&%&^%&^f3%%%".Sha256()) },
                AllowedScopes = new List<string>
                {
                    ApiScope,
                    ReadScope,
                    WriteScope,
                    DeleteScope
                },
                AccessTokenLifetime = 86400
            },
        };
}