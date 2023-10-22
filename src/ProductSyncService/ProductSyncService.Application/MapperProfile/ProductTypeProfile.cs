using AutoMapper;
using ProductSyncService.Application.DTO;

namespace ProductSyncService.Application.MapperProfile;

public class ProductTypeProfile: Profile
{
    public ProductTypeProfile()
    {
        CreateMap<Domain.Entities.ProductType, ProductTypeDTO>().ConvertUsing(
            e => new ProductTypeDTO(
                e.Id,
                e.Name,
                e.Description,
                e.ParentProductTypeId ?? Guid.Empty,
                e.CreatedDate,
                e.UpdatedDate ?? DateTime.MinValue
            ));
    }
}