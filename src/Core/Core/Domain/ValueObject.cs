namespace Core.Domain;

// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
public abstract class ValueObject<T> : IEquatable<ValueObject<T>> where T : ValueObject<T>
{
    protected abstract IEnumerable<object> EqualityComponents { get; }

    bool IEquatable<ValueObject<T>>.Equals(ValueObject<T>? other)
    {
        return this.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject<T>)obj;
        return EqualityComponents.SequenceEqual(other.EqualityComponents);
    }

    public static bool Equals(ValueObject<T> left, ValueObject<T> right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            return false;

        return left != null && (ReferenceEquals(left, right) || left.Equals(right));
    }

    protected static bool NotEquals(ValueObject<T> left, ValueObject<T> right)
    {
        return !(Equals(left, right));
    }

    public static bool operator ==(ValueObject<T>? first, ValueObject<T>? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(ValueObject<T>? first, ValueObject<T>? second) => !(first == second);

    public override int GetHashCode() =>
        EqualityComponents.Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
}