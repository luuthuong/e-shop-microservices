using Domain.Entities;

namespace Domain.DTO;

public class ProductDTO: BaseDTO
{
    public string Name { get; set; }
    public long Count { get; set; }
    public Price Price { get; set; }
}

public record AddProductRequest(String Name, long Count, Guid CategoryId);
