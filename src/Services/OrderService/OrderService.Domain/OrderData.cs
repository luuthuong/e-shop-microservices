namespace Domain;

public record class OrderData(
    CustomerId CustomerId,
    QuoteId QuoteId,    
    Currency? Currency = null,
    IReadOnlyList<ProductItemData>? Items = null);

public record class ProductItemData()
{
    public required ProductId ProductId { get; set; }
    public string? ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public required Money UnitPrice { get; set; }
}
