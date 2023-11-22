using Core.Domain;

namespace ProductSyncService.Domain.Categories;

public sealed class CategoryId: StronglyTypeId<Guid>
{
    private CategoryId(){}
    private CategoryId(Guid value) : base(value)
    {
    }
    
    public static CategoryId From(Guid value) => new (value);

    public static IEnumerable<CategoryId> From(IList<Guid> values)
    {
        foreach (var item in values)
        {
            yield return From(item);
        }
    }
}