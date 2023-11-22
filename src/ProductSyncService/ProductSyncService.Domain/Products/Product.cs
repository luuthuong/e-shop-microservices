using Core.Domain;
using Core.Exception;
using Core.Results;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Moneys;

namespace ProductSyncService.Domain.Products;

public class Product: AggregateRoot<ProductId>
{
    public string Name { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    
    public Money? Price { get; private set; }
    
    public CategoryId? CategoryId { get; private set; }
    
    public bool Published { get; private set; }

    private Product()
    {
        
    }

    private Product(
        string name, 
         CategoryId categoryId,
        string description = "", 
        string shortDescription = "", 
        Money? price = null,
        bool published = false
        )
    {
        Id = ProductId.From(Guid.NewGuid());
        Name = name;
        CategoryId = categoryId;
        CreatedDate = DateTime.Now;
        Description = description;
        ShortDescription = shortDescription;
        Price = price;
        Published = published;
    }

    public static Product Create(
        string name, 
        CategoryId?  categoryId = null,
        string description = "", 
        string shortDescription = "",
        Money? price = null,
        bool published = false
        )
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainLogicException("Product name can not be null or whitespace.");
        
        return new Product(name,categoryId, description, shortDescription,  price, published);
    }

    
    public Result OnPublished()
    {
        if (Published)
            return Result.Failure(ProductError.AlreadyPublished);
        Published = true;
        return Result.Success();
    }

    public void UnPublished()
    {
        Published = false;
    }

    public void Update(string? name, string? description, bool? published, long? quantity, long? price)
    {
        Name = name ?? Name;
        Description = description ?? string.Empty;
        Published = published ?? false;
        UpdatedDate = DateTime.Now;
    }
}