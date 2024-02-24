namespace API.Requests.Products;

public record CreateProductRequest(
    string Name,
    Guid CategoryId,
    string Description,
    string ShortDescription
);