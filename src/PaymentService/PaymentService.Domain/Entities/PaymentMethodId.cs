using Core.Domain;

namespace Domain.Entities;

public class PaymentMethodId: StronglyTypeId<Guid>
{
    private PaymentMethodId(Guid value) : base(value)
    {
    }

    public static PaymentMethodId From(Guid value) => new(value);
    public static IEnumerable<PaymentMethodId> From(IList<Guid> values) => values.Select(From);
}