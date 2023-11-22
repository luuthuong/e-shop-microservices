using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Moneys;

namespace API.Requests.Products;

public record UpdateProductRequest(
    Guid CategoryId,
    string Name,
    string Description,
    string ShortDescription,
    Money  Price
    );