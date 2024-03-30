using System.Reflection;
using Core.Api;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Builder;

namespace Core.Infrastructure.Api;

public static class ApiExtension
{
    public static void AddApiEndpoints(this WebApplication app, Assembly assembly)
    {
        var types = assembly.GetLoadableTypes().Where(t => t is
            {
                IsClass: true,
                IsAbstract: false
            } && t.GetInterfaces().Any(itf => itf == typeof(IApiEndpoint))
        ).Select( t => (IApiEndpoint)Activator.CreateInstance(t)!);

        foreach (var apiEndpoint in types)
        {
            apiEndpoint.Register(app);
        }
    }
}