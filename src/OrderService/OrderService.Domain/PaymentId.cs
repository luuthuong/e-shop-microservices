using Core.Domain;

namespace Domain;

public class PaymentId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static PaymentId From(Guid value) => new (value);
}