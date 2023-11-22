namespace Core.EventBus;

public interface IEventConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken = default);
}