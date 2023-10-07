using Application.DTO;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Application.DTO;
using ProductSyncService.Domain.Entities;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.CQRS.Products.Queries;

public record GetProductsQuery() : IRequest<GetPagingProductResponse>;

public sealed class GetProductsQueryHandler: IRequestHandler<GetProductsQuery, GetPagingProductResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<GetPagingProductResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _appDbContext.Product.ToListAsync(cancellationToken);
        var productRes = _mapper.Map<IList<Product>, IList<ProductDTO>>(products);

        return new GetPagingProductResponse()
        {
            Success = true,
            Data = new PageResponse<ProductDTO>()
            {
                Data = productRes,
                Total = products.Count
            }
        };
    }
}