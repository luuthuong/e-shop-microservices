using Core.EventBus;
using Core.Infrastructure.Outbox.Worker;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Kafka.Workers;

public class KafkaBackgroundServiceWorker(
    ILogger<BackgroundServiceWorker> logger, 
    IMessageConsumer consumer) : BackgroundServiceWorker(logger, consumer.ConsumeAsync)
{
}