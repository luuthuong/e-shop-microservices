using System.Collections;
using Core.Domain;

namespace Domain.Entities;

public class PaymentOptionId: StronglyTypeId<Guid>
{
    private PaymentOptionId(Guid value) : base(value)
    {
    }

    public static PaymentOptionId From(Guid value) => new(value);
    public static IEnumerable<PaymentOptionId> From(IList<Guid> values) => values.Select(From);
}