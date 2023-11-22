using Core.CQRS.Query;
using ProductSyncService.Domain.Products;
using ProductSyncService.DTO.Products;

namespace ProductSyncService.Application.Products.Queries;

public class GetProductById: IQuery<ProductDTO>
{
    public ProductId ProductId { get; private set; }
    
    public CancellationToken CancellationToken { get; private set; }

    private GetProductById(ProductId productId, CancellationToken cancellationToken) =>
        (ProductId, CancellationToken) = (productId, cancellationToken);

    public static GetProductById Create(ProductId productId, CancellationToken cancellationToken = default) =>
        new(productId, cancellationToken);
}