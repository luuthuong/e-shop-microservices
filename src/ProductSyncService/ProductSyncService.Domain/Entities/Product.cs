using Core.BaseDomain;
using Core.Utils;
using MassTransit;
using Microsoft.AspNetCore.Http;
using ProductSyncService.Domain.EntityErrors;

namespace ProductSyncService.Domain.Entities;

public class Product: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    
    public Guid ProductTypeId { get; set; }
    public virtual ProductType? ProductType { get; set; }
    
    public bool Published { get; private set; }
    
    public long Price { get; private set; }
    
    public long Quantity { get; private set; }

    private Product()
    {
        
    }

    private Product(
        string name, 
        Guid productTypeId,
        string description = "", 
        string shortDescription = "", 
        long quantity = 0, 
        bool published = false,
        long price = 0
        ) =>
        (
            Name,
            ProductTypeId,
            CreatedDate, 
            Description, 
            ShortDescription, 
            Quantity, 
            Published,
            Price
            ) = (
            name,
            productTypeId,
            DateTime.Now, 
            description, 
            shortDescription, 
            quantity, 
            published,
            price
        );

    public static Product Create(
        string name, 
        Guid productTypeId,
        string description = "", 
        string shortDescription = "", 
        long quantity = 0, 
        bool published = false, 
        long price = 0)
    {
        if (InValid(name))
            throw new ArgumentNullException(nameof(name));
        
        return new Product(name, productTypeId, description, shortDescription, quantity, published, price);
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
        Quantity = quantity ?? Quantity;
        Price = price ?? Price;
    }
    
    private static bool InValid(string name)
    {
        return string.IsNullOrEmpty(name);
    }

}