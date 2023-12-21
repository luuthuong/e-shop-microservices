using Core.Domain;

namespace ProductSyncService.Domain.Products;

public class ProductId: StronglyTypeId<Guid>
{
    private ProductId(){}
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