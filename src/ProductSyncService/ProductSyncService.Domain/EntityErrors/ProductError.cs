using Core.Utils;

namespace ProductSyncService.Domain.EntityErrors;

public static class ProductError
{
    public static Error AlreadyPublished => new("Product.AlreadyPublished", "Product has been already published");

    public static Error NotFound => new("Product.NotFound", "Product Not Exist");
}