using Core.Domain;

namespace Domain;

public class CustomerId: StronglyTypeId<Guid>
{
    private CustomerId(Guid value) : base(value)
    {
    }

    public static CustomerId From(Guid value) => new(value);
}