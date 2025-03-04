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
            throw new ArgumentNullException("Token Issuer wasn't found.");

        var integrationHttpSettings = configuration.GetSection("IntegrationHttpSettings") ??
                                      throw new ArgumentNullException("IntergrationHttpSettings wasn't found.");

        services.Configure<TokenIssuerSettings>(tokenIssuerSettings);
        services.Configure<IntegrationHttpSetting>(integrationHttpSettings);

        services.AddHttpClient();

        services.AddHttpClient(HttpClientNames.Default);

        services.AddHttpClient(HttpClientNames.ApiGateWay, client =>
        {
            var uriString = configuration.GetSection("ApiGatewayAddress").Value;
            if (uriString != null)
                client.BaseAddress = new System.Uri(uriString);
        });

        services.AddHttpClient(HttpClientNames.Authority, client =>
        {
            var uriString = configuration.GetSection("TokenIssuerSettings:Authority").Value;
            if (uriString != null)
                client.BaseAddress = new System.Uri(uriString);
        });

        services.AddKeyedSingleton<IHttpRequest, HttpRequestHandler>(
            HttpClientNames.Default,
            (sp, _) => new HttpRequestHandler(sp.GetRequiredService<IHttpClientFactory>(), HttpClientNames.Default));

        services.AddKeyedSingleton<IHttpRequest, HttpRequestHandler>(
            HttpClientNames.ApiGateWay,
            (sp, _) => new HttpRequestHandler(sp.GetRequiredService<IHttpClientFactory>(), HttpClientNames.ApiGateWay)
        );

        services.AddKeyedSingleton<IHttpRequest, HttpRequestHandler>(
            HttpClientNames.Authority,
            (sp, _) => new HttpRequestHandler(sp.GetRequiredService<IHttpClientFactory>(), HttpClientNames.Authority)
        );

        services.AddTransient<ITokenService, TokenService>();
        return services;
    }
}