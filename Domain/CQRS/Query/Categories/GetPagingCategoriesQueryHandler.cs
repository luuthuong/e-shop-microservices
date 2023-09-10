using AutoMapper;
using Domain.Database.Interface;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.CQRS.Query.Categories;

public record GetPagingCategoriesQuery(PageRequest Request = null) : IRequest<PageResponse<CategoryDTO>>;

public class GetPagingCategoriesQueryHandler: IRequestHandler<GetPagingCategoriesQuery, PageResponse<CategoryDTO>>
{
    private readonly IMapper _mapper;
    private readonly IAppDbContext _dbContext;

    public GetPagingCategoriesQueryHandler(IMapper mapper, IAppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<PageResponse<CategoryDTO>> Handle(GetPagingCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Category.Select(c => _mapper.Map<Category, CategoryDTO>(c)).ToListAsync(cancellationToken);
        return new PageResponse<CategoryDTO>
        {
            Data = result,
            Total = result.Count,
            TotalPage = default
        };
    }
}
