using Core.EF;

namespace ProductSyncService.Domain.Products;

public interface IProductRepository: IRepository<Product>
{ 
    Task<IEnumerable<Product>> GetListAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken = default);
}