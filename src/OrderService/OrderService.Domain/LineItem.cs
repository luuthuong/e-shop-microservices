using Core.Domain;
using Core.Exception;

namespace Domain;

public class LineItem: ValueObject<LineItem>
{
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }
    internal LineItem(ProductId productId, string productName, Money unitPrice, int quantity)
    {
        if (productId is null)
            throw new DomainLogicException("ProductId is required.");

        if (string.IsNullOrEmpty(productName))
            throw new DomainLogicException("Product name cannot be null or empty.");

        if (unitPrice is null)
            throw new DomainLogicException("Product unit price is required.");

        if (quantity <= 0)
            throw new DomainLogicException("Product quantity must be > 0.");

        ProductId = productId;
        ProductName = productName;
        Price = unitPrice;
        Quantity = quantity;
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return ProductId;
            yield return ProductName;
            yield return Price;
            yield return Quantity;
        }
    }
}