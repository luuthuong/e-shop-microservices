﻿
using IdentityModel.Client;

namespace Core.Identity;

public interface ITokenService
{
    Task<TokenResponse?> GetApplicationTokenAsync(TokenIssuerSettings settings);
    Task<TokenResponse> GetUserTokenAsync(TokenIssuerSettings settings, string userName, string password);
    Task<string?> GetUserTokenFromHttpContextAsync();

    Task<TokenRevocationResponse> RevokeTokenFromHttpContext(TokenIssuerSettings issuerSetting);
}