﻿using Microsoft.AspNetCore.Authorization;

namespace Core.Identity;

public static class AuthPolicyBuilder
{
    public static AuthorizationPolicy M2MAccess =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "eshop-api.scope")
            .Build();

    public static AuthorizationPolicy CanRead =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "read")
            .Build();

    public static AuthorizationPolicy CanWrite =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "write")
            .Build();

    public static AuthorizationPolicy CanDelete =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "delete")
            .Build();
}
