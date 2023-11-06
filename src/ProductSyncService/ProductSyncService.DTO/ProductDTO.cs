﻿using Core.Mediator;
using ProductSyncService.Application.DTO;

namespace ProductSyncService.DTO;

public class ProductDTO: BaseDTO
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public record AddProductRequest(string Name, Guid CategoryId, string Description);

public record UpdateProductRequest(string Name, bool Published = true, string Description = "", string ShortDescription= "");

public record AddProductResponse: BaseResponse<ProductDTO>;
public record GetPagingProductResponse: BaseResponse<PageResponse<ProductDTO>>;