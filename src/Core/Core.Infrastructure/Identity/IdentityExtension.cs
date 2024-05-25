using Core.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure;

public static class IdentityExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var tokenIssuerSettings = services.BuildServiceProvider().GetRequiredService<IOptions<TokenIssuerSettings>>();


        if (tokenIssuerSettings.Value is null || string.IsNullOrEmpty(tokenIssuerSettings.Value.Authority))
            throw new ArgumentNullException("TokenIssuerSettings:Authority section was not found");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.Authority = tokenIssuerSettings.Value.Authority;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = false
            };
        });

        return services;
    }

    public static string IdentityServerAddress(this TokenIssuerSettings tokenIssuer) => $"{tokenIssuer.Authority}/connect/token";
}
