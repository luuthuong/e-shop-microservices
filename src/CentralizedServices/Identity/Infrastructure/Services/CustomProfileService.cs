﻿using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Identity.Domains;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Services;

public class CustomProfileService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory)
    : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = (await userManager.FindByIdAsync(sub))!;

        var userClaims = await userClaimsPrincipalFactory.CreateAsync(user);

        var claims = userClaims.Claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new(JwtClaimTypes.Email, user.Email));

        if (userManager.SupportsUserRole)
        {
            var roles = await userManager.GetRolesAsync(user);
            
            foreach (var roleName in roles)
            {
                claims.Add(new(JwtClaimTypes.Role, roleName));
                if (!roleManager.SupportsRoleClaims)
                    continue;
                
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    claims.AddRange(await roleManager.GetClaimsAsync(role));
                }
            }
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}