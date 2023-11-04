using Core.BaseRepository;
using ProductSyncService.Domain.Entities;

namespace ProductSyncService.Infrastructure.Database.Interfaces;

public  interface IProductRepository: IRepository<Product>
{ 
    Task<IEnumerable<Product>> GetListAsync(CancellationToken cancellationToken = default);
}