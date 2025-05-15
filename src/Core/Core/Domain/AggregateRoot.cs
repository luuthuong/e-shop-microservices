using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Core.Domain;

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = [];

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public int Version { get; private set; } = -1;

    public IReadOnlyCollection<DomainEvent> GetUncommittedEvents() => _domainEvents.AsReadOnly();

    public void ClearUncommittedEvents() => _domainEvents.Clear();

    protected void RaiseEvent(DomainEvent @event, bool isNew = true)
    {
        dynamic dynamicEvent = @event;

        var methodName = "Apply";
        var method = GetType().GetMethod(
            methodName,
            BindingFlags.NonPublic | BindingFlags.Instance, null,
            [@event.GetType()], null
        );

        if (method == null)
            throw new InvalidOperationException($"No method found for event type {GetType().Name}");

        method.Invoke(this, [dynamicEvent]);

        if (isNew)
        {
            _domainEvents.Add(@event);
        }
        else
        {
            Version++;
        }
    }

    public void LoadFromHistory(IEnumerable<DomainEvent> history)
    {
        foreach (var @event in history)
        {
            RaiseEvent(@event, false);
        }
    }
    
    protected int NextVersion => Version + 1;
}