using Core.Domain;

namespace Domain;

public class ProductLineItem: ValueObject<ProductLineItem>
{
    public ProductId ProductId { get; private set; }
    
    public string ProductName { get; private set; }
    public Money Price { get; private set; }
    public Currency Currency { get; private set; }
    public int Quantity { get; private set; }
    protected override IEnumerable<object> EqualityComponents { get; }
}