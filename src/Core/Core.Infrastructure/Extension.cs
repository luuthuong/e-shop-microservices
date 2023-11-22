using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}