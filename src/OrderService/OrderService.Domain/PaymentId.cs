using Core.Domain;

namespace Domain;

public class PaymentId: StronglyTypeId<Guid>
{
    public static PaymentId From(Guid value) => new (value);

    public PaymentId(Guid value) : base(value)
    {
    }
}