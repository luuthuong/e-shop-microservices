using Core.Domain;
using Core.Exception;

namespace Domain;

public class ProductItem : ValueObject<ProductItem>
{
    private ProductItem()
    {
    }

    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public Currency Currency { get; private set; }
    public int Quantity { get; private set; }

    internal ProductItem(
        ProductId productId, string productName, Money unitPrice, int quantity, Currency currency)
    {
        if (productId is null)
        {
            throw new DomainRuleException("ProductId is required.");
        }

        if (string.IsNullOrEmpty(productName))
        {
            throw new DomainRuleException("Product name cannot be null or empty.");
        }

        if (unitPrice is null)
        {
            throw new DomainRuleException("Product unit price is required.");
        }

        if (currency is null)
        {
            throw new DomainRuleException("Currency is required.");
        }

        if (quantity <= 0)
        {
            throw new DomainRuleException("Product quantity must be > 0.");
        }

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Currency = currency;
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return ProductId;
            yield return ProductName;
            yield return UnitPrice;
            yield return Currency;
            yield return Quantity;
        }
    }
}