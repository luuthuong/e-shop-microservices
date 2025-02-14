using Core.Domain;

namespace Domain;

public sealed class OrderId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static OrderId From(Guid value) => new OrderId(value);
}
