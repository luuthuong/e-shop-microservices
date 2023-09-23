using Core.Mediator;

namespace Application.DTO;

public class CategoryDTO: BaseDTO
{
    public string Name { get; set; } = String.Empty;
}

public record AddCategoryRequest(string Name);

public record AddCategoryResponse: BaseResponse<CategoryDTO>;

public record GetPagingCategoryResponse : BaseResponse<PageResponse<CategoryDTO>>;
