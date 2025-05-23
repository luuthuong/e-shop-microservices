﻿using System.IdentityModel.Tokens.Jwt;
using Core.Identity;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Infrastructure.Identity;

public class TokenService(
    IMemoryCache cache,
    IHttpContextAccessor httpContextAccessor,
    IHttpClientFactory factory
) : ITokenService
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    private const string ApplicationKey = "ApplicationToken";

    public async Task<TokenResponse?> GetApplicationTokenAsync(TokenIssuerSettings settings)
    {
        var isStoredToken = cache.TryGetValue(ApplicationKey, out TokenResponse? tokenResponse);

        if (!isStoredToken)
            tokenResponse = await RequestApplicationTokenAsync(settings);

        if (isStoredToken && IsTokenExpired(tokenResponse))
            tokenResponse = await RequestApplicationTokenAsync(settings);

        return tokenResponse;
    }

    public Task<TokenResponse> GetUserTokenAsync(TokenIssuerSettings settings, string userName, string password)
    {
        return _httpClient.RequestPasswordTokenAsync(
            new()
            {
                Address = settings.GetIdentityTokenUrl(),
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                Scope = settings.Scope,
                GrantType = ClientGrantTypes.Password,
                UserName = userName,
                Password = password
            }
        );
    }

    public async Task<string?> GetUserTokenFromHttpContextAsync() => await httpContextAccessor.HttpContext?.GetTokenAsync("access_token")!;
    public async Task<TokenRevocationResponse> RevokeTokenFromHttpContext(TokenIssuerSettings issuerSetting)
    {
        var token = await GetUserTokenFromHttpContextAsync();
        var result = await _httpClient.RevokeTokenAsync(
            new TokenRevocationRequest()
            {
                Address = issuerSetting.GetIdentityTokenUrl(),
                Token = token
            }
        );
        return result;
    }

    private async Task<TokenResponse?> RequestApplicationTokenAsync(TokenIssuerSettings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
            new()
            {
                Address = settings.GetIdentityTokenUrl(),
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                Scope = settings.Scope
            }
        );

        if (tokenResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            cache.Set(ApplicationKey, tokenResponse);

        return tokenResponse;
    }

    private bool IsTokenExpired(TokenResponse? tokenResponse)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(tokenResponse?.AccessToken);

        return jwtSecurityToken.ValidTo < DateTime.UtcNow.AddSeconds(10);
    }
}