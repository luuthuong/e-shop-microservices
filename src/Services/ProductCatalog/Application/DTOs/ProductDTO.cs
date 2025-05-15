using ProductCatalog.Infrastructure.Models;

namespace ProductCatalog.Application.DTOs;

public class ProductDTO
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string PictureUrl { get; private set; }
    public int AvailableStock { get; private set; }
    public bool IsActive { get; private set; }
    public string Category { get; private set; }


    public static ProductDTO FromProduct(ProductReadModel product)
    {
        return new ProductDTO()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            AvailableStock = product.AvailableStock,
            Category = product.Category,
            PictureUrl = product.PictureUrl,
            IsActive = product.IsActive
        };
    }
}