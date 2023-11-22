namespace Core.Domain;
public abstract class AggregateRoot<TKey>: BaseEntity, IAggregateRoot where TKey: StronglyTypeId<Guid>
{
    public TKey Id { get; protected set; } = default!;

    private readonly List<IDomainEvent> _domainEvents = new();
    public void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
}
