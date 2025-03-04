using Core.Exception;

namespace Domain;

public class OrderLine
{
    public ProductItem ProductItem { get; private set; }
    
    private OrderLine(){}

    internal static OrderLine Create(ProductItem productItem)
    {
        if (productItem.ProductId is null)
            throw new DomainRuleException("ProductId is required.");

        if (string.IsNullOrEmpty(productItem.ProductName))
            throw new DomainRuleException("Product name is required.");
        
        if (productItem.UnitPrice is null)
            throw new DomainRuleException("Product unit price is required.");
        
        if (productItem.Quantity <= 0)
            throw new DomainRuleException("Product quantity must be > 0.");

        return new OrderLine(productItem);
    }

    private OrderLine(ProductItem productItem) => ProductItem = productItem;
}