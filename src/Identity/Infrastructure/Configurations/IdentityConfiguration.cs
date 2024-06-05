using Core;
using Core.Identity;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.Infrastructure.Configurations;

public abstract class IdentityConfiguration
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiResource> ApiResources => [
        new("eshop-api")
        {
            Scopes = [
                IdentityValueScopes.ApiScope,
                IdentityValueScopes.ReadScope,
                IdentityValueScopes.WriteScope,
                IdentityValueScopes.DeleteScope
            ]
        }
    ];

    public static IEnumerable<ApiScope> ApiScopes => [
            new(IdentityValueScopes.ApiScope, "Eshop api"),
            new(IdentityValueScopes.ReadScope, "Read your data."),
            new(IdentityValueScopes.WriteScope, "Write your data."),
            new(IdentityValueScopes.DeleteScope, "Delete your data."),
    ];

    public static IEnumerable<Client> GetClients(IdentityTokenIssuerSettings tokenIssuer)
    {
        yield return new()
        {
            ClientId = tokenIssuer.UserClient.Id,
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            RequireClientSecret = true,
            ClientSecrets = [new Secret(tokenIssuer.UserClient.Secret.Sha256())],
            AllowedScopes = [
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                IdentityValueScopes.ReadScope,
                IdentityValueScopes.WriteScope,
                IdentityValueScopes.DeleteScope
            ],
            AccessTokenLifetime = tokenIssuer.UserClient.AccessTokenLifetime
        };

        yield return new()
        {
            ClientId = tokenIssuer.ApplicationClient.Id,
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            RequireClientSecret = true,
            ClientSecrets = [new(tokenIssuer.ApplicationClient.Secret.Sha256())],
            AllowedScopes = [IdentityValueScopes.ApiScope, IdentityValueScopes.ReadScope, IdentityValueScopes.WriteScope, IdentityValueScopes.DeleteScope],
            AccessTokenLifetime = tokenIssuer.ApplicationClient.AccessTokenLifetime
        };
    }
}