using AutoMapper;
using ProductSyncService.Domain.Entities;
using ProductSyncService.DTO;

namespace ProductSyncService.Application.MapperProfile;

public sealed class ProductTypeProfile: Profile
{
    public ProductTypeProfile()
    {
        CreateMap<ProductType, ProductTypeDTO>().ConvertUsing(
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