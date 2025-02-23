using ProductSyncService.Domain.Products;

namespace ProductSyncService.Domain.Quotes;

public record class QuoteItemData(
    QuoteId QuoteId,
    ProductId ProductId,
    string ProductName,
    Money ProductPrice,
    int Quantity);