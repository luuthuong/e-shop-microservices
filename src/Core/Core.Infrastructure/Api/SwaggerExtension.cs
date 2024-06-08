using Asp.Versioning.ApiExplorer;
using Core.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Core.Infrastructure.Api;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection service, IConfiguration configuration,
        string swaggerSettingSection = "SwaggerSettings")
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        var swaggerGenerateSetting = configuration.GetSection(swaggerSettingSection).Get<SwaggerGenerateSetting>();

        if (swaggerGenerateSetting is null)
            return service;

        var (version, title, description) = swaggerGenerateSetting;
        
        service.AddSwaggerGen(
            s =>
            {
                s.SwaggerDoc(
                    $"v{version}", new OpenApiInfo()
                    {
                        Version = $"v{version}",
                        Title = title,
                        Description = description,
                        Contact = new() { Name = "to me", Email = "nguyen.thuong.work@gmail.com" },
                        License = new()
                            { Name = "MIT", Url = new Uri("https://github.com/luuthuong/e-shop-microservices") }
                    }
                );
                
                s.AddSecurityDefinition(
                    "Bearer",
                    new()
                    {
                        Name = "Authorization",
                        Description = "Jwt authorization header using the Bear scheme.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    }
                );
                
                s.AddSecurityRequirement(
                    new()
                    {
                        {
                            new()
                            {
                                Reference = new()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                },
                                Scheme = "oauth2",
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header
                            },
                            []
                        }
                    }
                );
                
                s.EnableAnnotations();
            }
        );

        return service;
    }

    public static IApplicationBuilder UseAppSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            config =>
            {
                IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
                foreach (var description in descriptions)
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpperInvariant();
                    config.SwaggerEndpoint(url, name);
                }
            });
        return app;
    }
}