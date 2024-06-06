using Core.CQRS.Command;
using Core.EF;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Application.Products;

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.CategoryId,
            request.Description,
            request.ShortDescription
        );
        await productRepository.InsertAsync(product, cancellationToken:  cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}