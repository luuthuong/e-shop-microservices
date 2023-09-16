using Application.DTO;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Query.Products;

public record GetProductsQuery() : IRequest<IEnumerable<ProductDTO>>;

public sealed class GetProductsQueryHandler: IRequestHandler<GetProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _appDbContext.Product.ToListAsync(cancellationToken);
        return _mapper.Map<IList<Product>, IList<ProductDTO>>(products);
    }
}