using Core.Domain;

namespace Domain;

public sealed class ShipmentId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static ShipmentId From(Guid value) => new ShipmentId(value);
}
