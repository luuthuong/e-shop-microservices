namespace API.Requests.Products;

public record ProductCreateRequest(
    string Name,
    Guid CategoryId,
    string Description,
    string ShortDescription
);