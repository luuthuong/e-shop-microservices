using System.ComponentModel.DataAnnotations;
using ProductCatalog.Domain.Product.Events;

namespace ProductCatalog.Infrastructure.Models;

public sealed class ProductReadModel
{
    [Key] public Guid Id { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string PictureUrl { get; private set; }
    public int AvailableStock { get; private set; }
    public bool IsActive { get; private set; }
    public string Category { get; private set; }

    public ProductReadModel()
    {
    }

    internal void Apply(ProductCreatedEvent @event)
    {
        Id = @event.AggregateId;
        Name = @event.Name;
        Description = @event.Description;
        Price = @event.Price;
        PictureUrl = @event.PictureUrl;
        AvailableStock = @event.AvailableStock;
        Category = @event.Category;
        IsActive = true;
    }

    internal void Apply(ProductUpdatedEvent @event)
    {
        Name = @event.Name;
        Description = @event.Description;
        Price = @event.Price;
        PictureUrl = @event.PictureUrl;
        AvailableStock = @event.AvailableStock;
        Category = @event.Category;
    }

    internal void Apply(ProductActivatedEvent @event)
    {
        IsActive = true;
    }

    internal void Apply(ProductDeactivatedEvent @event)
    {
        IsActive = false;
    }

    public void Apply(StockReservedEvent @event)
    {
    }
}