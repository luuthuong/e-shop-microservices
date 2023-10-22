using Core.BaseDbContext;
using Infrastructure.Database.Interfaces;

namespace Infrastructure.Database;

public class UnitOfWorkBehavior<TRequest, TResponse>: BaseUnitOfWorkBehavior<TRequest, TResponse> where TRequest: notnull
{
    public UnitOfWorkBehavior(IAppDbContext context) : base(context)
    {
    }
}