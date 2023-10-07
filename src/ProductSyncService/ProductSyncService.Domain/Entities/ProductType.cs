using System.Collections;
using Core.BaseDomain;

namespace ProductSyncService.Domain.Entities;

public class ProductType: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid? ParentProductTypeId { get; private set; }

    public virtual IList<Product> Products { get; set; }

    private ProductType(){}

    private ProductType(string name, Guid? parentId, string description) =>
    (Name, CreatedDate, ParentProductTypeId, Description) = (name, DateTime.Now, parentId, description);

    public static ProductType Create(string name, Guid? parentId, string description)
    {
        if (InValid(name))
            throw new ArgumentNullException(nameof(name));
        return new ProductType(name, parentId, description);
    }
    
    private static bool InValid(string name)
    {
        return string.IsNullOrEmpty(name);
    }
}