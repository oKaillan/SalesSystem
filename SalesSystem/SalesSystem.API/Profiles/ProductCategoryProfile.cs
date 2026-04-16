using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos;

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
        CreateMap<ProductCategory, CategoryDto>();
    }
}
