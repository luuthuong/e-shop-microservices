using Core.Domain;

namespace Domain;

public sealed class PaymentId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static PaymentId From(Guid value) => new PaymentId(value);
}
