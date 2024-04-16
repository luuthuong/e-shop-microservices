using Core.CQRS.Command;

namespace ProductSyncService.Application.Products;

internal sealed class UpdateProductCommandHandler: ICommandHandler<UpdateProductCommand>
{
    public Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}