using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Core.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
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

    public static RouteGroupBuilder MapGroupWithApiVersioning(this WebApplication app, int apiVersion)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(apiVersion))
            .ReportApiVersions()
            .Build();

        return app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
    }

    public static IApplicationBuilder UseSwagger(this WebApplication app, bool onlyDevelopment)
    {
        if (onlyDevelopment && !app.Environment.IsDevelopment())
            return app;

        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
                foreach (var description in descriptions)
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        return app;
    }

    public static IApplicationBuilder MapApiEndpoints(this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var apiEndpoints = app.Services.GetRequiredService<IEnumerable<IApiEndpoint>>();

        IEndpointRouteBuilder routeBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var apiEndpoint in apiEndpoints)
        {
            apiEndpoint.Register(routeBuilder);
        }

        return app;
    }
}