using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MassTransits;

public static class MassTransitExtension
{
    public static IServiceCollection MassTransitRegistry(this IServiceCollection services)
    {
        return services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
            
        });
    }
}