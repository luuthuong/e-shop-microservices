using Microsoft.AspNetCore.Authorization;

namespace Identity;

public record class AuthorizeRequirement(string ClaimName, string ClaimValue ): IAuthorizationRequirement;
