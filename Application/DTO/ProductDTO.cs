using Core.Mediator;
using Domain.Entities;

namespace Application.DTO;

public class ProductDTO: BaseDTO
{
    public string Name { get; set; }
    public long Count { get; set; }
    public Price Price { get; set; }
}

public record AddProductRequest(string Name, long Count, Guid CategoryId);
public record AddProductResponse: BaseResponse<ProductDTO>;
public record GetPagingProductResponse: BaseResponse<PageResponse<ProductDTO>>;