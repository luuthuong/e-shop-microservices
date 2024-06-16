using Core.Configs;
using Core.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure;

public static class IdentityExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        TokenIssuerSettings tokenIssuerSettings)
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

    public static string GetIdentityTokenUrl(this TokenIssuerSettings tokenIssuer) =>
        $"{tokenIssuer.Authority}/connect/token";

    public static string GetIdentityUserInfoUrl(this TokenIssuerSettings tokenIssuer) =>
        $"{tokenIssuer.Authority}/connect/userinfo";

    public static string IdentityRevocationTokenUrl(this TokenIssuerSettings tokenIssuerSetting) =>
        $"{tokenIssuerSetting.Authority}/connect/revocation";

    public static void ThrowIfFailure(this IdentityResult result, string msg = "")
    {
        if (!result.Succeeded)
            return;
        throw new ApplicationException(msg);
    }

    public static void ThrowIfFailure(this IdentityResult result, Func<IEnumerable<IdentityError>?, string> format)
    {
        if (!result.Succeeded)
            return;
        throw new ApplicationException(format(result.Errors));
    }
}