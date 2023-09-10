namespace Domain.DTO;

public class CategoryDTO: BaseDTO
{
    public string Name { get; set; } = String.Empty;
}

public record AddCategoryRequest(string Name);