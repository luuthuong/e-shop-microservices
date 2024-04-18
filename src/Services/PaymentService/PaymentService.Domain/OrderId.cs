using Core.Domain;

namespace Domain;

public sealed class OrderId: StronglyTypeId<Guid>
{

    public static OrderId From(Guid value) => new(value);

    public static IEnumerable<OrderId> From(IList<Guid> values) => values.Select(From);
    
    private OrderId(Guid value) : base(value)
    {
        
    }
}