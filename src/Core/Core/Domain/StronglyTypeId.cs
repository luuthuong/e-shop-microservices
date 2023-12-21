using Core.Exception;

namespace Core.Domain;

public abstract class StronglyTypeId<T>: ValueObject<StronglyTypeId<T>>
{
    public T Value { get; private set; }

    protected StronglyTypeId()
    {
        
    }

    protected StronglyTypeId(T value)
    {
        if (value == null || value.Equals(Guid.Empty))
            throw new DomainRuleException("id must be valid.");
        Value = value;
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Value!;
        }
    }
    
    protected bool Equals(StronglyTypeId<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Value!);
    }
    
    public static bool operator == (StronglyTypeId<T>? first, StronglyTypeId<T>? second) =>
        first is not null && second is not null && first.Equals(second);
    
    public static bool operator !=(StronglyTypeId<T>? first, StronglyTypeId<T>? second) => !(first == second);
}