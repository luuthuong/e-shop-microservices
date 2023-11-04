using AutoMapper;
using ProductSyncService.Application.DTO;
using ProductSyncService.Application.Helpers;
using ProductSyncService.DTO;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.CQRS.ProductTypes.Commands;

public record CreateProductTypeCommand(string Name,Guid? ParentProductType, string Description = "") : BaseRequest<CreateProductTypeResponse>;


internal sealed class CreateProductTypeCommandHandler : BaseRequestHandler<CreateProductTypeCommand,CreateProductTypeResponse>
{
    public CreateProductTypeCommandHandler(IMapper mapper, IAppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override Task<CreateProductTypeResponse> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = Domain.Entities.ProductType.Create(
            request.Name,
            request.ParentProductType,
            request.Description
        );
        var result = DBContext.ProductType.Add(entity).Entity;
        return Task.FromResult(
            new CreateProductTypeResponse()
            {
                Success = true,
                Data = Mapper.Map<Domain.Entities.ProductType, ProductTypeDTO>(result),
            }
        );
    }
}

