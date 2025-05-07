using Core.EventBus;
using Core.Infrastructure.Kafka.Consumer;
using Core.Infrastructure.Kafka.Serialization;
using Core.Infrastructure.Kafka.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.Kafka;

public static class KafkaExtension
{
    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaConsumerConfig>(configuration.GetSection("KafkaConsumer"));
        services.AddSingleton(typeof(JsonEventSerializer<>));
        services.TryAddSingleton<IMessageConsumer, KafkaConsumer>();
        services.AddHostedService<KafkaBackgroundServiceWorker>();
        return services;
    }
}