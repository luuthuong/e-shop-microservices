using Core.Domain;
using Core.Exception;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Domain.Product;

public class ProductAggregate : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int AvailableStock { get; private set; }
    public int ReservedStock { get; private set; }
    public string PictureFileName { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Brand { get; private set; }
    public bool IsActive { get; private set; }

    public ProductAggregate()
    {
    }

    public ProductAggregate(
        Guid id,
        string name,
        string description,
        decimal price,
        string currencyCode,
        int availableStock,
        string pictureFileName,
        Guid categoryId,
        string brand)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainRuleException("Product name is required");

        if (price <= 0)
            throw new DomainRuleException("Product price must be greater than zero");

        Id = id;
        Name = name;
        Description = description;
        Price = new Money(price, currencyCode);
        AvailableStock = availableStock > 0 ? availableStock : 0;
        ReservedStock = 0;
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
        Brand = brand;
        IsActive = true;
    }

    public void UpdateDetails(
        string name,
        string description,
        decimal price,
        string currencyCode,
        string pictureFileName,
        Guid categoryId,
        string brand)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainRuleException("Product name is required");

        if (price <= 0)
            throw new DomainRuleException("Product price must be greater than zero");

        Name = name;
        Description = description;
        Price = new Money(price, currencyCode);
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
        Brand = brand;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than zero");

        AvailableStock += quantity;
    }

    public bool ReserveStock(int quantity, Guid orderId)
    {
        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than zero");

        if (!HasSufficientStock(quantity))
            return false;

        AvailableStock -= quantity;
        ReservedStock += quantity;

        return true;
    }

    public bool ConfirmStockReservation(int quantity, Guid orderId)
    {
        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than zero");

        if (ReservedStock < quantity)
            return false;

        ReservedStock -= quantity;

        return true;
    }

    public bool ReleaseStockReservation(int quantity, Guid orderId)
    {
        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than zero");

        if (ReservedStock < quantity)
            return false;

        ReservedStock -= quantity;
        AvailableStock += quantity;

        return true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
    }

    private bool HasSufficientStock(int quantity)
    {
        return AvailableStock >= quantity;
    }
}