using Confluent.Kafka;
using Core.EventBus;
using Core.Infrastructure.Kafka.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace Core.Infrastructure.Kafka.Consumer;

public class KafkaConsumer(
    IEventPublisher eventPublisher,
    ILogger<KafkaConsumer> logger) : IEventConsumer
{
    private readonly IConsumer<string, INotification> _consumer;

    public KafkaConsumer(IEventPublisher eventPublisher,
        ILogger<KafkaConsumer> logger,
        IOptions<KafkaConsumerConfig> kafkaConsumerConfig,
        JsonEventSerializer<INotification> serializer
    ) : this(eventPublisher, logger)
    {
        _consumer = new ConsumerBuilder<string, INotification>(
                new ConsumerConfig()
                {
                    GroupId = kafkaConsumerConfig.Value.GroupId,
                    BootstrapServers = kafkaConsumerConfig.Value.BootstrapServers,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false
                }
            )
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(serializer)
            .Build();
        logger.LogInformation("Subcribe topics: {Topics}", kafkaConsumerConfig.Value.Topics!);
        _consumer.Subscribe(kafkaConsumerConfig.Value.Topics);
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
            await ConsumeNextMessage(_consumer, cancellationToken);
    }

    private async Task ConsumeNextMessage(IConsumer<string, INotification> consumer,
        CancellationToken cancellationToken)
    {
        var retryPolicy = Policy.Handle<KafkaException>().WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(5));
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromHours(1));
        var policyWrap = Policy.WrapAsync(timeoutPolicy, retryPolicy);

        await policyWrap.ExecuteAsync(async () =>
            {
                var @event = consumer.Consume();
                if (@event is null)
                {
                    logger.LogInformation("Unable deserialize event message from kafka");
                    await Task.CompletedTask;
                }

                logger.LogInformation($"Dispatching event: {@event}");
                await eventPublisher.PublishAsync(@event!.Message.Value, cancellationToken);
                consumer.Commit();
            })
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    var ex = task.Exception.Flatten().InnerException;
                    logger.LogError(task.Exception, "Unable to consume message from kafka: {Message} {StackTrace} ",
                        ex?.Message, ex?.StackTrace);
                    _consumer.Close();
                }
            }, cancellationToken);
    }
}