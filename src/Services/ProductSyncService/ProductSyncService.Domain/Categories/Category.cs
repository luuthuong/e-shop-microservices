using Core.Domain;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Domain.Categories;

public class Category: BaseEntity<CategoryId>
{
    public string Name { get; set; } = string.Empty;

    public virtual IList<Product> Products { get; set; } = new List<Product>();

    private Category()
    {
        
    }
    
    private Category(string name)
    {
        Id = CategoryId.From(Guid.NewGuid());
        Name = name;
        CreatedDate = DateTime.Now;
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }
}