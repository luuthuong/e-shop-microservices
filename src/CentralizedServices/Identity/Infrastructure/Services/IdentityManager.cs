using System.Net;
using Core;
using Core.Identity;
using Core.Infrastructure;
using Core.Results;
using Identity.API.Requests;
using Identity.Domains;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Services;

public class IdentityManager(
    ITokenService tokenRequest,
    UserManager<User> userManager,
    IOptions<IdentityTokenIssuerSettings> issuerSettings,
    RoleManager<IdentityRole> roleManager
)
    : IIdentityManager
{
    private readonly IdentityTokenIssuerSettings _issuerSettings = issuerSettings.Value;

    public async Task<Result<TokenResponse>> AuthUserByCredentials(LoginRequest request)
    {
        var response = await tokenRequest.GetUserTokenAsync(
            new()
            {
                Authority = _issuerSettings.Authority,
                ClientId = _issuerSettings.UserClient.Id,
                ClientSecret = _issuerSettings.UserClient.Secret,
                Scope = _issuerSettings.UserClient.Scope,
            },
            request.Email,
            request.Password
        );

        if (response.HttpStatusCode == HttpStatusCode.BadRequest)
            return Result<TokenResponse>.Failure(AuthenticationError.AuthenticateFailed);
        
        return Result<TokenResponse>.Success(response);
    }

    public async Task<IdentityResult> RegisterNewUser(RegisterUserRequest request)
    {

        if (request.Password.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(request.Password));
        
        if (!request.Password.Equals(request.PasswordConfirm))
            throw new Exception("password confirm is not matched.");
        
        await AddDefaultRoles();

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        result.ThrowIfFailure($"Create user with mail {user.Email} failed.");

        result = await userManager.AddToRoleAsync(user, RoleConstants.Customer);

        result.ThrowIfFailure($"Can't add role for {user.Email}");

        result = await userManager.AddClaimsAsync(user, user.GetUserCustomerClaims());

        result.ThrowIfFailure($"Can't add claims for {user.Email}");

        return result;
    }

    public async Task<IdentityResult> RegisterUserAdmin(RegisterUserRequest request)
    {
        await AddDefaultAdminRole();

        User user = new()
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(result.Errors.First().Description);

        result = await userManager.AddClaimsAsync(user, user.GetUserAdminClaims());

        result.ThrowIfFailure($"Can't add claims for {user.Email}");

        result = await userManager.AddToRoleAsync(user, RoleConstants.Admin);

        result.ThrowIfFailure($"Can't add role for {RoleConstants.Admin}");

        return result;
    }

    private async Task AddDefaultRoles()
    {
        var clientRole = await roleManager.FindByNameAsync(RoleConstants.Customer);

        if (clientRole is null)
        {
            var result = await roleManager.CreateAsync(new(RoleConstants.Customer));
            result.ThrowIfFailure($"Can't add role {RoleConstants.Customer}");
        }
    }

    private async Task AddDefaultAdminRole()
    {
        var adminRole = await roleManager.FindByNameAsync(RoleConstants.Admin);

        if (adminRole is null)
        {
            var result = await roleManager.CreateAsync(new(RoleConstants.Admin));
            result.ThrowIfFailure($"Can't add role {RoleConstants.Admin}");
        }
    }
}