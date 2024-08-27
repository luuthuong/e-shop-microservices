namespace Core.Infrastructure.Kafka.Consumer;

public sealed class KafkaConsumerConfig
{
    public required string BootstrapServers { get; init; }
    public required string GroupId { get; init; }
    public string[]? Topics { get; init; }
}
