using Microsoft.AspNetCore.Authorization;

namespace Identity.Application;

internal sealed class AuthorizeRequirementHandler : AuthorizationHandler<AuthorizeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AuthorizeRequirement requirement)
    {
        var claim = context.User.Claims.SingleOrDefault(c => c.Type == requirement.ClaimName)!;

        if (claim.Value.Contains(requirement.ClaimValue))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}