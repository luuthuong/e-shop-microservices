using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Infrastructure.Identity;

public class TokenRequester(
    IMemoryCache cache,
    IHttpContextAccessor httpContextAccessor,
    IHttpClientFactory factory)
    : ITokenRequester
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    private const string ApplicationKey = "ApplicationToken";

    // Caching application token
    public async Task<TokenResponse?> GetApplicationTokenAsync(TokenIssuerSettings settings)
    {
        var isStoredToken = cache.TryGetValue(ApplicationKey, out TokenResponse? tokenResponse);

        if (!isStoredToken)        
            tokenResponse = await RequestApplicationTokenAsync(settings);      
        
        if (isStoredToken && IsTokenExpired(tokenResponse))
            tokenResponse = await RequestApplicationTokenAsync(settings);
        
        return tokenResponse;       
    }

    public async Task<TokenResponse> GetUserTokenAsync(TokenIssuerSettings settings, string userName, string password)
    {
        var identityServerAddress = $"{settings.Authority}/connect/token";
        var response = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = identityServerAddress,
            ClientId = settings.ClientId,
            ClientSecret = settings.ClientSecret,
            Scope = settings.Scope,
            GrantType = "password",
            UserName = userName,
            Password = password
        });

        return response;
    }

    public async Task<string?> GetUserTokenFromHttpContextAsync() =>
        await httpContextAccessor.HttpContext?.GetTokenAsync("access_token")!;

    private async Task<TokenResponse?> RequestApplicationTokenAsync(TokenIssuerSettings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var identityServerAddress = $"{settings.Authority}/connect/token";
        var tokenResponse = await _httpClient
            .RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = identityServerAddress,
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                Scope = settings.Scope
            });

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