using Core.Domain;

namespace PaymentProcessing.Domain;

public class OrderItem : ValueObject<OrderItem>
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public Money Price { get; }
        
    public OrderItem(Guid productId, string productName, int quantity, Money price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
                
        ProductId = productId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        Quantity = quantity;
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }

    protected override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return ProductId;
            yield return ProductName;
            yield return Quantity;
            yield return Price;
        }
    }
}