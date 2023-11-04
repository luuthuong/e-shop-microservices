using Core.BaseDomain;
using Core.BaseRepository;

namespace ProductSyncService.Infrastructure.Database.Repository;

public abstract class AppRepository<TEntity>: BaseRepository<AppDbContext, TEntity> where TEntity: BaseEntity
{
    protected AppRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}