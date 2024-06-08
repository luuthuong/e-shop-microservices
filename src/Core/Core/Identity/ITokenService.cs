
using IdentityModel.Client;

namespace Core.Identity;

public interface ITokenService
{
    Task<TokenResponse?> GetApplicationTokenAsync(ClientTokenIssuerSetting settings);
    Task<TokenResponse> GetUserTokenAsync(ClientTokenIssuerSetting settings, string userName, string password);
    Task<string?> GetUserTokenFromHttpContextAsync();
}