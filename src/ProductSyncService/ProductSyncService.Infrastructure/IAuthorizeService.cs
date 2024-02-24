using MediatR;
using Core.Results;

namespace ProductSyncService.Infrastructure;

public interface IAuthorizeService< T> where T: notnull
{
    public Task<Result> Handle(T message, CancellationToken cancellationToken = default);
}
