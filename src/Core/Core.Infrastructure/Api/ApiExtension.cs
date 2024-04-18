using System.Reflection;
using Core.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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