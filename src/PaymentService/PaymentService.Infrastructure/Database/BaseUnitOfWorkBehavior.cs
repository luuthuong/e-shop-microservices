using Infrastructure.Database.Interfaces;

namespace Infrastructure.Database;

public class BaseUnitOfWorkBehavior<TRequest, TResponse>: Core.Infrastructure.CQRS.BaseUnitOfWorkBehavior<TRequest, TResponse> where TRequest: notnull
{
    public BaseUnitOfWorkBehavior(IAppDbContext context) : base(context)
    {
    }
}