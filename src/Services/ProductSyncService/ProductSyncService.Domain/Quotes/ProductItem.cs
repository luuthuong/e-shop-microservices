using Core.Domain;
using Core.Exception;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Domain.Quotes;

public class ProductItem : ValueObject<ProductItem>
{
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }


    internal ProductItem(ProductId productId, string productName, Money unitPrice, int quantity)
    {
        if (productId is null)
            throw new DomainRuleException("ProductId is required.");

        if (string.IsNullOrEmpty(productName))
            throw new DomainRuleException("Product name cannot be null or empty.");

        if (unitPrice is null)
            throw new DomainRuleException("Product unit price is required.");

        if (quantity <= 0)
            throw new DomainRuleException("Product quantity must be > 0.");

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void ChangeQuantity(int quantity)
    {
        if (quantity == 0)
            throw new DomainRuleException("The product quantity must be at last 1.");

        Quantity = quantity;
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return ProductId;
            yield return ProductName;
            yield return UnitPrice;
            yield return Quantity;
        }
    }
}