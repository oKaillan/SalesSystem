using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;

namespace SalesSystem.API.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product,  ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<PatchProductDto, Product>();
        CreateMap<Product, PatchProductDto>();
    }
}
