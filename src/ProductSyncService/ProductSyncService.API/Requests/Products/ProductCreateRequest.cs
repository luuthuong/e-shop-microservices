namespace API.Requests.Products;

internal sealed record ProductCreateRequest(
    string Name,
    Guid CategoryId,
    string Description,
    string ShortDescription
);