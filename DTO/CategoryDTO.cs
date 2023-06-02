namespace Domain.DTO;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public record AddCategoryRequest(string Name);