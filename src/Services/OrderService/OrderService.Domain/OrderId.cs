using Core.Domain;

namespace Domain.Orders;

public sealed class OrderId: StronglyTypeId<Guid>
{
    private OrderId(Guid value): base(value)
    {
    }

    public static OrderId From(Guid value) => new (value);

    public static IEnumerable<OrderId> From(IEnumerable<Guid> values) => values.Select(From);
}