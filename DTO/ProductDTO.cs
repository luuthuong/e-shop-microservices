using Domain.Entities;

namespace Domain.DTO;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long Count { get; set; }
    public Price Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public record AddProductRequest(String Name, long Count, Guid CategoryId);
