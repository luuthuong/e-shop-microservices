using System.Reflection;
using AutoMapper;
using Core.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.AutoMapper;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var assemblies = typeof(Profile).GetUsedAssemblies(true).ToList();
        return services.AddAutoMapper(assemblies);
    }
}