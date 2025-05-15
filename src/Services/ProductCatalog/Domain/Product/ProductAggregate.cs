using Core.Domain;
using Core.Exception;
using ProductCatalog.Domain.Product.Events;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Domain.Product;

public class ProductAggregate : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string PictureUrl { get; private set; }
    public int AvailableStock { get; private set; }
    public bool IsActive { get; private set; }
    public string Category { get; private set; }

    public ProductAggregate()
    {
    }

    public ProductAggregate(
        string name,
        string description,
        decimal price,
        string pictureUrl,
        int availableStock,
        string category)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        PictureUrl = pictureUrl;
        AvailableStock = availableStock;
        IsActive = true;
        Category = category;

        RaiseEvent(new ProductCreatedEvent(
            Name,
            Description,
            Price,
            PictureUrl,
            AvailableStock,
            Category,
            Id,
            NextVersion));
    }

    public static ProductAggregate Create(string name, string description, decimal price, int availableStock,
        string pictureUrl, string category)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainRuleException("Product name cannot be empty");

        if (price <= 0)
            throw new DomainRuleException("Product price must be greater than zero");

        if (availableStock < 0)
            throw new DomainRuleException("Product available stock cannot be less than zero");

        return new ProductAggregate(name, description, price, pictureUrl, availableStock, category);
    }

    public void UpdateDetails(
        string name,
        string description,
        decimal price,
        string pictureUrl,
        string category)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainRuleException("Product name cannot be empty");

        if (price <= 0)
            throw new DomainRuleException("Product price must be greater than zero");

        Name = name;
        Description = description;
        Price = price;
        PictureUrl = pictureUrl;
        Category = category;

        RaiseEvent(new ProductUpdatedEvent(Name, Description, Price, AvailableStock, PictureUrl, Category, Id, NextVersion));
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than 0");

        AvailableStock += quantity;

        RaiseEvent(new StockAddedEvent(quantity, AvailableStock, Id, NextVersion));
    }

    public bool ReserveStock(Guid orderId, int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainRuleException("Order ID cannot be empty");

        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than 0");

        if (AvailableStock < quantity)
        {
            RaiseEvent(new OutOfStockEvent(orderId, quantity, AvailableStock, Id, NextVersion));
            return false;
        }

        AvailableStock -= quantity;

        RaiseEvent(new StockReservedEvent(orderId, quantity, AvailableStock, Id, NextVersion));
        return true;
    }

    public void ReleaseStock(Guid orderId, int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainRuleException("Order ID cannot be empty");

        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than 0");

        AvailableStock += quantity;

        RaiseEvent(new StockReleasedEvent(orderId, quantity, AvailableStock, Id, NextVersion));
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        RaiseEvent(new ProductActivatedEvent(Id, NextVersion));
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        RaiseEvent(new ProductDeactivatedEvent(Id, NextVersion));
    }

    void Apply(ProductCreatedEvent @event)
    {
        Id = @event.AggregateId;
        Name = @event.Name;
        Description = @event.Description;
        Price = @event.Price;
        PictureUrl = @event.PictureUrl;
        AvailableStock = @event.AvailableStock;
        IsActive = true;
        Category = @event.Category;
    }

    void Apply(ProductUpdatedEvent @event)
    {
        Name = @event.Name;
        Description = @event.Description;
        Price = @event.Price;
        PictureUrl = @event.PictureUrl;
        Category = @event.Category;
    }

    void Apply(StockAddedEvent @event)
    {
        AvailableStock = @event.NewAvailableStock;
    }

    void Apply(StockReservedEvent @event)
    {
        AvailableStock -= @event.Quantity;
    }

    void ApplyEvent(ProductActivatedEvent @event)
    {
        IsActive = true;
    }

    void Apply(ProductDeactivatedEvent @event)
    {
        IsActive = false;
    }

    void Apply(StockReleasedEvent @event)
    {
        AvailableStock += @event.Quantity;
    }
}