namespace ProductSyncService.DTO.Products;

public class ProductDTO: BaseDTO
{
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Name { get; set; }
}
