using Identity.API.Requests;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domains;

public interface IIdentityManager
{
    Task<TokenResponse> AuthUserByCredentials(LoginRequest request);
    Task<IdentityResult> RegisterNewUser(RegisterUserRequest request);

    Task<IdentityResult> RegisterUserAdmin(RegisterUserRequest request);
}
