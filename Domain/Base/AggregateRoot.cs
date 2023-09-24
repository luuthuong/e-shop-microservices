using Domain.Entities;

namespace Domain.Base;

public abstract class AggregateRoot: BaseEntity, IAggregateRoot
{

    private readonly List<IDomainEvent> _domainEvents = new();
    public void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
}
