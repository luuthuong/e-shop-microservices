namespace Domain.Base;

public interface IAggregateRoot
{
    void RaiseDomainEvent(IDomainEvent @event);
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}