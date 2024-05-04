namespace Core.Domain;
public abstract class AggregateRoot<TKey>: BaseEntity<TKey>, IAggregateRoot 
    where TKey: StronglyTypeId<Guid>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
}
