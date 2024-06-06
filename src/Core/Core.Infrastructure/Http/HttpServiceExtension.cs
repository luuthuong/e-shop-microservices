using Core.Http;
using Core.Identity;
using Core.Infrastructure.Identity;
using Core.Integration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Http;

public static class HttpServiceExtension
{
    public static IServiceCollection AddHttpService(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenIssuerSettings = configuration.GetSection("TokenIssuerSettings");
        if (tokenIssuerSettings is null)
            throw new ArgumentNullException("TokenIssuer wasn't found.");

        var integrationHttpSettings = configuration.GetSection("IntegrationHttpSettings") ?? throw new ArgumentNullException("IntergrationHttpSettings wasn't found.");

        services.Configure<TokenIssuerSettings>(tokenIssuerSettings);
        services.Configure<IntegrationHttpSetting>(integrationHttpSettings);

        services.AddHttpClient();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IHttpRequest, HttpRequestHandler>();

        return services;
    }
}