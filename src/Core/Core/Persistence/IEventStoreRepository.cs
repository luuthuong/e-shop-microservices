using Core.Domain;

namespace Core.Persistence;

public interface IEventStoreRepository<TAggregate> where TAggregate: IAggregateRoot
{
    Task<long> AppendEventsAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    void AppendToOutbox(IDomainEvent @event);
}