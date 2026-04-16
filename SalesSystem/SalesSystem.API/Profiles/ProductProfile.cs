using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;

namespace SalesSystem.API.Profiles;
/// <summary>
/// Class responsible to map Products
/// </summary>
public class ProductProfile : Profile
{
    /// <summary>
    /// Maps Product using AutoMapper
    /// </summary>
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>().ForMember(dest => dest.Categories, opt => opt.Ignore());
        CreateMap<Product, GetProductDto>();
        CreateMap<GetProductDto, Product>();
        CreateMap<PatchProductDto, Product>();
        CreateMap<Product, PatchProductDto>();
    }
}
