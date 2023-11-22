
namespace Infrastructure.Database;

public class UnitOfWorkBehavior<TRequest, TResponse>: Core.Infrastructure.CQRS.UnitOfWorkBehavior<TRequest, TResponse>
{
    public UnitOfWorkBehavior(AppDbContext context) : base(context)
    {
    }
}