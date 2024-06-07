using Microsoft.AspNetCore.Authorization;

namespace Identity.Application;

public record class AuthorizeRequirement(string ClaimName, string ClaimValue ): IAuthorizationRequirement;
