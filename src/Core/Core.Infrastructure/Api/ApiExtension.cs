using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Core.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Core.Infrastructure.Api;

public static class ApiExtension
{
    public static IServiceCollection AddApiEndpoints(this IServiceCollection service, Assembly assembly)
    {
        ServiceDescriptor[] apiEndpointDescriptors = assembly.DefinedTypes.Where(
            type => type is
            {
                IsAbstract: false,
                IsInterface: false
            } && type.IsAssignableTo(typeof(IApiEndpoint))
        ).Select(
            type => ServiceDescriptor.Transient(typeof(IApiEndpoint), type)
        ).ToArray();

        service.TryAddEnumerable(apiEndpointDescriptors);
        return service;
    }

    public static IServiceCollection AddVersioningApi(this IServiceCollection services, int apiVersion = 1)
    {
        services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(apiVersion);
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });
        return services;
    }

    public static IApplicationBuilder UseMinimalApi(this WebApplication app,
        IConfiguration configuration, string swaggerSettingSection = "SwaggerSettings")
    {
        var swaggerGenerateSetting = configuration.GetSection(swaggerSettingSection).Get<SwaggerGenerateSetting>();

        if (swaggerGenerateSetting is null)
            return app;

        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(swaggerGenerateSetting.Version))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder routeGroupBuilder = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        var apiEndpoints = app.Services.GetRequiredService<IEnumerable<IApiEndpoint>>();

        IEndpointRouteBuilder routeBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var apiEndpoint in apiEndpoints)
        {
            apiEndpoint.Register(routeBuilder);
        }
        
        return app;
    }
}