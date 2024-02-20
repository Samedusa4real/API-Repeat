using AutoMapper;
using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.ProductDtos;
using Shop.Core.Entities;

namespace Shop.Services.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductGetDto>();
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetAllItemDto>()
                .ForMember(dest => dest.HasDiscount, m => m.MapFrom(s => s.DiscountPercent > 0));

            CreateMap<BrandPostDto, Brand>();
            CreateMap<Brand, BrandGetAllItemDto>();
            CreateMap<Brand, BrandInProductGetDto>();
        }
    }
}
