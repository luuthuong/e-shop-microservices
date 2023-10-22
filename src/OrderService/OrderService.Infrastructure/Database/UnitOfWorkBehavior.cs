using Core.BaseDbContext;
using Infrastructure.Database.Interface;

namespace Infrastructure.Database;

public class UnitOfWorkBehavior<TRequest, TResponse>: BaseUnitOfWorkBehavior<TRequest, TResponse>
{
    public UnitOfWorkBehavior(IAppDbContext context) : base(context)
    {
    }
}