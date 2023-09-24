using Application.DTO;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Queries;

public record GetPagingCategoriesQuery(PageRequest? Request = null) : BaseRequest<GetPagingCategoryResponse>;

public class GetPagingCategoriesQueryHandler: IRequestHandler<GetPagingCategoriesQuery, GetPagingCategoryResponse>
{
    private readonly IMapper _mapper;
    private readonly IAppDbContext _dbContext;

    public GetPagingCategoriesQueryHandler(IMapper mapper, IAppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<GetPagingCategoryResponse> Handle(GetPagingCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Category.Select(c => _mapper.Map<Category, CategoryDTO>(c)).ToListAsync(cancellationToken);
        return new GetPagingCategoryResponse()
        {
            Success = true,
            Data = new PageResponse<CategoryDTO>()
            {
                Data = result,
                Total = result.Count,
                TotalPage = default
            }
        };
    }
}
