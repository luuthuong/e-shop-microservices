using Core.Domain;

namespace Domain;

public sealed class CustomerId: StronglyTypeId<Guid>
{
    public static CustomerId From(Guid value) => new(value);

    public static IEnumerable<CustomerId> From(IList<Guid> values) => values.Select(From);
    
    private CustomerId(Guid value) : base(value)
    {
        
    }
}