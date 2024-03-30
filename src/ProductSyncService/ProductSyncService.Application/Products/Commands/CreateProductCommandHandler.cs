using Core.CQRS.Command;
using Core.EF;
using ProductSyncService.Domain.Products;
using ProductSyncService.Infrastructure.Persistence;

namespace ProductSyncService.Application.Products.Commands;

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork<ProductSyncDbContext> unitOfWork
) : ICommandHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.CategoryId,
            request.Description,
            request.ShortDescription);
        await productRepository.InsertAsync(product, cancellationToken:  cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}