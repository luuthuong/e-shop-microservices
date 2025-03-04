using Core.Exception;

namespace ProductSyncService.Domain.Quotes;

public class QuoteItem
{
    public ProductItem ProductItem { get; private set; }

    internal static QuoteItem Create(ProductItem productItem)
    {
        if (productItem is null)
            throw new DomainRuleException("ProductItem is required");

        return new QuoteItem(productItem);
    }

    internal void ChangeQuantity(int quantity)
    {
        if (quantity == 0)
            throw new DomainRuleException("The product quantity must be at last 1.");

        ProductItem.ChangeQuantity(quantity);
    }

    private QuoteItem(ProductItem productItem) => ProductItem = productItem;
}