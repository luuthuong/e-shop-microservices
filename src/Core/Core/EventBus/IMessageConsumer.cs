namespace Core.EventBus;

public interface IMessageConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken = default);
}