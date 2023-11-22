using Core.Domain;

namespace Domain.Customers;

public class CustomerId: StronglyTypeId<Guid>
{
    private CustomerId(Guid value) : base(value)
    {
    }

    public static CustomerId From(Guid value) => new(value);
}