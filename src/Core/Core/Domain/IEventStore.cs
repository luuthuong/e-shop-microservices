namespace Core.Domain;

public interface IEventStore<T> where T : AggregateRoot, new()
{
    Task SaveAsync(T aggregate);
    Task SaveAsync(IEnumerable<T> aggregateItems);
    Task<T?> LoadAsync(Guid aggregateId);
    
    Task<IEnumerable<T>> LoadAsync(IEnumerable<Guid> aggregateIds);
    /// <summary>
    ///  Append a domain event to the outbox for message broker.
    /// </summary>
    /// <param name="event">Domain event</param>
    /// <returns></returns>
    Task AppendToOutboxAsync(IntegrationEvent @event);
    Task AppendToOutboxAsync(IEnumerable<IntegrationEvent> @event);
    Task AppendToOutboxAsync(DomainEvent @event);
    /// <summary>
    ///  Append a domain events to the outbox for message broker.
    /// </summary>
    /// <param name="events">List domain events</param>
    /// <returns></returns>
    Task AppendToOutboxAsync(IEnumerable<DomainEvent> @events);

}