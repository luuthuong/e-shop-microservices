using Application.DTO;
using Core.Mediator;

namespace ProductSyncService.Application.DTO;

public class ProductDTO: BaseDTO
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record AddProductRequest(string Name, Guid CategoryId, string Description);
public record AddProductResponse: BaseResponse<ProductDTO>;
public record GetPagingProductResponse: BaseResponse<PageResponse<ProductDTO>>;