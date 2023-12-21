using MediatR.Pipeline;

namespace ProductSyncService.Infrastructure.Persistence;
public class UnitOfWorkBehavior<TRequest, TResponse>: Core.Infrastructure.CQRS.UnitOfWorkBehavior<TRequest, TResponse> where TRequest: notnull
{
    public UnitOfWorkBehavior(ProductSyncDbContext context) : base(context)
    {
    }
}

