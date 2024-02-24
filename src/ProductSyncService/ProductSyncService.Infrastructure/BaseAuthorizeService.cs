using Core.Results;

namespace ProductSyncService.Infrastructure;

public abstract class BaseAuthorizeService<T>: IAuthorizeService<T>
{
    public abstract Task<Result> Handle(T message, CancellationToken cancellationToken = default);
}