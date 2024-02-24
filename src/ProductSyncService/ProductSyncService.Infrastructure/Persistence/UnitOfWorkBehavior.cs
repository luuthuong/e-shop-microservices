using Core.Infrastructure.CQRS;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ProductSyncService.Infrastructure.Persistence;

public class UnitOfWorkBehavior<TRequest, TResponse>(
    ProductSyncDbContext context,
    IServiceProvider serviceProvider
)
    : BaseUnitOfWorkBehavior<TRequest, TResponse>(context)
    where TRequest : notnull
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var service = serviceProvider.GetService<IAuthorizeService<TRequest>>();
        if (service is not null)
        {
            var result = await service.Handle(request, cancellationToken);
            if (result.IsFailure)
                throw new Exception(result.Error.Description);
        }

        return await base.Handle(request, next, cancellationToken);
    }
}