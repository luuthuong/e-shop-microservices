using Infrastructure.Database.Interfaces;

namespace Infrastructure.Database;

public class UnitOfWorkBehavior<TRequest, TResponse>: Core.Infrastructure.CQRS.UnitOfWorkBehavior<TRequest, TResponse> where TRequest: notnull
{
    public UnitOfWorkBehavior(IAppDbContext context) : base(context)
    {
    }
}