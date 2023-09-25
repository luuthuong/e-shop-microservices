using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.AutoMapper;

public static class AutoMapperExtensions
{
    public static IServiceCollection AutoMapperConfigure(this IServiceCollection services)
    {
        return services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}