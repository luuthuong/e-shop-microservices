using Core.Domain;

namespace Domain.Payments;

public class PaymentId : StronglyTypeId<Guid>
{
    public static PaymentId From(Guid value) => new(value);

    public static IEnumerable<PaymentId> From(IList<Guid> values) => values.Select(From);

    private PaymentId(Guid value) : base(value)
    {
    }
}