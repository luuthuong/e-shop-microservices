using Microsoft.AspNetCore.Authorization;

namespace Core.Identity;

public static class AuthPolicyBuilder
{
    public static AuthorizationPolicy M2MAccess =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", IdentityValueScopes.ApiScope)
            .Build();


    public static AuthorizationPolicy CanRead =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", IdentityValueScopes.ReadScope)
            .Build();

    public static AuthorizationPolicy CanWrite =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", IdentityValueScopes.WriteScope)
            .Build();

    public static AuthorizationPolicy CanDelete =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", IdentityValueScopes.DeleteScope)
            .Build();

    public static AuthorizationPolicy Admin => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Combine(CanWrite)
        .RequireRole(RoleConstants.Admin)
        .Build();
}