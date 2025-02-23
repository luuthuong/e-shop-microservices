using Core.EventBus;

namespace Core.EF;

public interface IEventOutboxRepository
{
    Task InsertAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
    
    Task InsertAsync(IEnumerable<IntegrationEvent> @events, CancellationToken cancellationToken = default);
}