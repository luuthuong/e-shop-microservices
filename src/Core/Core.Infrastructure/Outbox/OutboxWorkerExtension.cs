using System;
using Core.Infrastructure.Outbox.Worker;
using Core.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.Outbox;

public static class OutboxWorkerExtension
{
    public static IServiceCollection AddDebeziumWorker(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));
        
        var debeziumSetting = configuration.GetSection("DebeziumSetting");
        
        if (debeziumSetting is null)
            throw new ArgumentNullException(nameof(debeziumSetting));
        
        services.Configure<DebeziumSetting>(debeziumSetting);
        services.TryAddSingleton<IDebeziumConnectorConfiguration, DebeziumConnectorConfiguration>();
        services.AddHostedService<DebeziumServiceWorker>();
        return services;
    }
}
