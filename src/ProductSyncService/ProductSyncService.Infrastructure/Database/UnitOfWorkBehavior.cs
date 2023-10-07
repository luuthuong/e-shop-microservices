using Core.BaseDbContext;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Infrastructure.Database;
public class UnitOfWorkBehavior<TRequest, TResponse>: BaseUnitOfWorkBehavior<TRequest, TResponse> where TRequest: notnull
{
    public UnitOfWorkBehavior(IAppDbContext context) : base(context)
    {
    }
}