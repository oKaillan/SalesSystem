using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.ProductCategoryDto;
using SalesSystem.Shared.Database.Responses;

namespace SalesSystem.API.Profiles;
/// <summary>
/// Class responsible to map Product Categories
/// </summary>
public class ProductCategoryProfile : Profile
{
/// <summary>
/// Maps Product Categories using AutoMapper
/// </summary>
    public ProductCategoryProfile()
    {
        CreateMap<ProductCategory, GetCategoryDto>();
        CreateMap<ProductCategory, CreateCategoryDto>();
        CreateMap<CreateCategoryDto, ProductCategory>();
        CreateMap<CreateCategoryDto, GetCategoryDto>();
        CreateMap<PagedResult<ProductCategory>, PagedResult<GetCategoryDto>>();
        CreateMap<PagedResult<GetCategoryDto>, PagedResult<ProductCategory>>();
    }
}
