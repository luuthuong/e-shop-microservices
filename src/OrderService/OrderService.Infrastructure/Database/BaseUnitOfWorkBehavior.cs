
namespace Infrastructure.Database;

public class BaseUnitOfWorkBehavior<TRequest, TResponse>: Core.Infrastructure.CQRS.BaseUnitOfWorkBehavior<TRequest, TResponse>
{
    public BaseUnitOfWorkBehavior(AppDbContext context) : base(context)
    {
    }
}