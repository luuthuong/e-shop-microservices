using Confluent.Kafka;
using Core.Domain;
using Core.EventBus;
using Core.Infrastructure.Kafka.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace Core.Infrastructure.Kafka.Consumer;

public class KafkaConsumer(
    IEventBus eventPublisher,
    ILogger<KafkaConsumer> logger) : IMessageConsumer
{
    private readonly IConsumer<string, IIntegrationEvent> _consumer;

    public KafkaConsumer(
        IEventBus eventPublisher,
        ILogger<KafkaConsumer> logger,
        IOptions<KafkaConsumerConfig> kafkaConsumerConfig,
        JsonEventSerializer<IIntegrationEvent> serializer
    ) : this(eventPublisher, logger)
    {
        _consumer = new ConsumerBuilder<string, IIntegrationEvent>(
                new ConsumerConfig()
                {
                    GroupId = kafkaConsumerConfig.Value.GroupId,
                    BootstrapServers = kafkaConsumerConfig.Value.BootstrapServers,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                }
            )
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(serializer)
            .Build();
        
        logger.LogInformation("Subscribe topics: {Topics}", kafkaConsumerConfig.Value.Topics);
        
        _consumer.Subscribe(kafkaConsumerConfig.Value.Topics);
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
            await ConsumeNextMessage(_consumer, cancellationToken);
    }

    private async Task ConsumeNextMessage(IConsumer<string, IIntegrationEvent> consumer,
        CancellationToken cancellationToken)
    {
        var retryPolicy = Policy.Handle<KafkaException>().WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(30));
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

                if (@event?.Message.Value is null)
                {
                    logger.LogInformation("Message deserialized value is null");
                    consumer.Commit();
                    return;
                }

                logger.LogInformation($"Dispatching event: {@event}...");
                
                await eventPublisher.PublishAsync(@event!.Message.Value);
                consumer.Commit();
            })
            .ContinueWith(task =>
            {
                if (!task.IsFaulted) 
                    return;
                
                var ex = task.Exception.Flatten().InnerException;
                logger.LogError(task.Exception, "Unable to consume message from kafka: {Message} {StackTrace} ",
                    ex?.Message, ex?.StackTrace);
                
                _consumer.Close();
            }, cancellationToken);
    }
}