namespace ProductCatalog.Application.DTOs;

public class ReleaseStockDTO
{
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
}