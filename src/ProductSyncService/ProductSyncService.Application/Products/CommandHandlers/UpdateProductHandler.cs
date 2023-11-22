using Core.CQRS.Command;
using ProductSyncService.Application.Products.Commands;

namespace ProductSyncService.Application.Products.CommandHandlers;

internal sealed class UpdateProductHandler: ICommandHandler<UpdateProductById>
{
    public Task Handle(UpdateProductById request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}