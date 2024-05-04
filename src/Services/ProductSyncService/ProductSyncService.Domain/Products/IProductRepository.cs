using Core.EF;

namespace ProductSyncService.Domain.Products;

public interface IProductRepository: IRepository<Product, ProductId>
{ 
    Task<IEnumerable<Product>> GetListAsync(CancellationToken cancellationToken = default);
}