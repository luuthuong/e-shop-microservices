using Core.Domain;

namespace Domain;

public sealed class ProductId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static ProductId From(Guid value) => new ProductId(value);
}
