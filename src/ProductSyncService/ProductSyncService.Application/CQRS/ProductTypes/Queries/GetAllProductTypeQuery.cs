using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Application.DTO;
using ProductSyncService.Application.Helpers;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.CQRS.ProductTypes.Queries;

public record GetAllProductTypeQuery : BaseRequest<IEnumerable<ProductTypeDTO>>;

internal sealed class GetAllProductTypeQueryHandler : BaseRequestHandler<GetAllProductTypeQuery, IEnumerable<ProductTypeDTO>>
{
    public GetAllProductTypeQueryHandler(IMapper mapper, IAppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<IEnumerable<ProductTypeDTO>> Handle(GetAllProductTypeQuery request, CancellationToken cancellationToken)
    {
        var entities = await DBContext.ProductType.ToListAsync(cancellationToken);
        return Mapper.Map<IList<Domain.Entities.ProductType>, IList<ProductTypeDTO>>(entities);
    }
}
