
using IdentityModel.Client;

namespace Core.Identity;

public interface ITokenRequest
{
    Task<TokenResponse?> GetApplicationTokenAsync(TokenIssuerSettings settings);
    Task<TokenResponse> GetUserTokenAsync(TokenIssuerSettings settings, string userName, string password);
    Task<string?> GetUserTokenFromHttpContextAsync();
}