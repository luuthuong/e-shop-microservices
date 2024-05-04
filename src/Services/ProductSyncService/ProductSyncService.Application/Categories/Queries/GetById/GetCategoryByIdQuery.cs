using Core.CQRS.Query;
using ProductSyncService.Domain.Categories;
using ProductSyncService.DTO.Categories;

namespace ProductSyncService.Application.Categories;

public sealed record GetCategoryByIdQuery(CategoryId CategoryId) : IQuery<CategoryDTO>;