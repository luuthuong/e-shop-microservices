using Core.Mediator;

namespace ProductSyncService.DTO;

public record ProductTypeDTO(
    Guid Id,
    string Name, 
    string Description, 
    Guid ParentProductTypeId,
    DateTime CreatedDate, 
    DateTime UpdatedDate
    );

public record CreateProductTypeRequest(
    string Name,
    Guid? ParentId,
    string Description = "");

public record CreateProductTypeResponse: BaseResponse<ProductTypeDTO>
{
    
}