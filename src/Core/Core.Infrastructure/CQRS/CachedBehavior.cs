using MediatR;

namespace Core.Infrastructure.CQRS;

public class CachedBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return next();
    }
}