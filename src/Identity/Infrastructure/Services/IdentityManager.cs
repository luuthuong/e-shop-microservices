using System.Net;
using System.Security.Claims;
using Core;
using Core.Identity;
using Identity.API.Requests;
using Identity.Domains;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Services;

public class IdentityManager(
    ITokenRequest tokenRequest,
    UserManager<User> userManager,
    IOptions<TokenIssuerSettings> issuerSettings,
    RoleManager<IdentityRole> roleManager)
    : IIdentityManager
{
    private readonly TokenIssuerSettings _issuerSettings = issuerSettings.Value;

    public async Task<TokenResponse> AuthUserByCredentials(LoginRequest request)
    {
        var response = await tokenRequest.GetUserTokenAsync(
            _issuerSettings,
            request.Email,
            request.Password
        );

        if (response.HttpStatusCode == HttpStatusCode.BadRequest)
            throw new AuthenticateFailedException($"Invalid username or password.");

        return response;
    }

    public async Task<IdentityResult> RegisterNewUser(RegisterUserRequest request)
    {
        await AddDefaultRoles();

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await userManager
            .CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(result.Errors.First().Description);

        result = await userManager
            .AddToRoleAsync(user, RoleConstants.Customer);

        if (!result.Succeeded)
            throw new ApplicationException($"Can't add role for {user.Email}");

        result = await userManager.AddClaimsAsync(user, GetUserCustomerClaims(user));

        if (!result.Succeeded)
            throw new ApplicationException($"Can't add claims for {user.Email}");

        return result;
    }

    public async Task<IdentityResult> RegisterUserAdmin(RegisterUserRequest request)
    {
        await AddDefaultAdminRole();

        User user = new(){
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
            throw new ApplicationException(result.Errors.First().Description);

        result = await userManager.AddClaimsAsync(user, GetUserAdminClaims(user));

        if (!result.Succeeded)
            throw new ApplicationException($"Can't add claims for {user.Email}");

        return result;
    }

    private async Task AddDefaultRoles()
    {
        var clientRole = await roleManager.FindByNameAsync(RoleConstants.Customer);

        if (clientRole is null)
        {
            var result = await roleManager.CreateAsync(new(RoleConstants.Customer));

            if (!result.Succeeded)
                throw new ApplicationException($"Can't add role {RoleConstants.Customer}");
        }
    }

    private async Task AddDefaultAdminRole(){
        var adminRole = await roleManager.FindByNameAsync(RoleConstants.Admin);

        if(adminRole is null){
            var result = await roleManager.CreateAsync(new(RoleConstants.Admin));

            if(result is {Succeeded: false})
                throw new ApplicationException(result.Errors.First().Description);
        }
    }

    private IEnumerable<Claim> GetUserAdminClaims(User user){
        yield return new(JwtClaimTypes.Name, user.UserName!);
        yield return new(JwtClaimTypes.Email, user.Email!);
        yield return new(JwtClaimTypes.Role, RoleConstants.Admin);
    }


    private IEnumerable<Claim> GetUserCustomerClaims(User user){
        yield return new(JwtClaimTypes.Name, user.UserName!);
        yield return new(JwtClaimTypes.Email, user.Email!);
        yield return new(JwtClaimTypes.Role, RoleConstants.Customer);
    }
}