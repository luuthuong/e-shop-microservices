using Core.Configs;
using Core.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure;

public static class IdentityExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, TokenIssuerSettings tokenIssuerSettings)
    {
        if (string.IsNullOrEmpty(tokenIssuerSettings.Authority))
            throw new ArgumentNullException("TokenIssuerSettings:Authority section was not found");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.Authority = tokenIssuerSettings.Authority;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static string IdentityServerAddress(this ClientTokenIssuerSetting tokenIssuer) => $"{tokenIssuer.Authority}/connect/token";
    public static string IdentityUserInfo(this TokenIssuerSettings tokenIssuer) => $"{tokenIssuer.Authority}/connect/userinfo";
}
