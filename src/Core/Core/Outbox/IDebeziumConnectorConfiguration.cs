namespace Core.Outbox;

public interface IDebeziumConnectorConfiguration
{
    Task ConfigureAsync(CancellationToken cancellationToken = default);
}