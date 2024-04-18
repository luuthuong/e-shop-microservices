using Core.Domain;

namespace Domain;

public sealed class ProductId: StronglyTypeId<Guid>
{
    private ProductId(Guid value) : base(value)
    {
    }
    
    public static ProductId From(Guid value) => new (value);

    public static IEnumerable<ProductId> From(IList<Guid> values)
    {
        foreach (var item in values)
        {
            yield return From(item);
        }
    }
}