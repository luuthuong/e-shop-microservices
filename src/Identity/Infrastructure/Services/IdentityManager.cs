using System.Net;
using System.Security.Claims;
using Core.Identity;
using Identity.API.Requests;
using Identity.Domains;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Services;

public class IdentityManager(
    ITokenRequester tokenRequester,
    UserManager<User> userManager,
    IOptions<TokenIssuerSettings> issuerSettings,
    RoleManager<IdentityRole> roleManager)
    : IIdentityManager
{
    private readonly TokenIssuerSettings _issuerSettings = issuerSettings.Value;

    public async Task<TokenResponse> AuthUserByCredentials(LoginRequest request)
    {
        var response = await tokenRequester.GetUserTokenAsync(
            _issuerSettings,
            request.Email,
            request.Password);

        if (response.HttpStatusCode == HttpStatusCode.BadRequest)
            throw new ApplicationException($"Invalid username or password.");

        return response;
    }

    public async Task<IdentityResult> RegisterNewUser(RegisterUserRequest request)
    {
        await AddDefaultRoles();

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
        };

        // Creating user
        var result = await userManager
            .CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new ApplicationException(result.Errors.First().Description);

        // Adding role
        result = await userManager
            .AddToRoleAsync(user, Roles.Customer);
        if (!result.Succeeded)
            throw new ApplicationException($"Can't add role for {user.Email}");

        // Adding claims
        result = await userManager.AddClaimsAsync(user,
            new Claim[]
            {
                new(JwtClaimTypes.Name, user.UserName),
                new(JwtClaimTypes.Email, user.Email),
                new(JwtClaimTypes.Role, Roles.Customer),
            });
        if (!result.Succeeded)
            throw new ApplicationException($"Can't add claims for {user.Email}");

        return result;
    }

    private async Task AddDefaultRoles()
    {
        var clientRole = await roleManager.FindByNameAsync(Roles.Customer);

        if (clientRole is null)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(Roles.Customer));

            if (!result.Succeeded)
                throw new ApplicationException($"Can't add role {Roles.Customer}");
        }

        await Task.CompletedTask;
    }
}