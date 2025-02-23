namespace Domain;

public record OrderData(
    CustomerId CustomerId,
    QuoteId QuoteId,    
    Currency? Currency = null,
    IReadOnlyList<ProductItemData>? Items = null);

public record ProductItemData()
{
    public required ProductId ProductId { get; set; }
    public required string ProductName { get; init; }
    public int Quantity { get; set; }
    public required Money UnitPrice { get; set; }
}
