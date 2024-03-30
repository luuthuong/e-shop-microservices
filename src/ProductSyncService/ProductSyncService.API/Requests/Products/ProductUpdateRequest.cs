using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Moneys;

namespace API.Requests.Products;

public record ProductUpdateRequest(
    Guid CategoryId,
    string Name,
    string Description,
    string ShortDescription,
    Money  Price
    );