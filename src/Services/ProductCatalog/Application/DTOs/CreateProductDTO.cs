namespace ProductCatalog.Application.DTOs;

public class CreateProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public int AvailableStock { get; set; }
    public string Category { get; set; }
}