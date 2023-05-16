using Api.Apps.AdminApi.Dtos.BrandDtos;
using Api.Apps;
using AutoMapper;
using Core.Entities;
using Api.Apps.ClientApi.Dtos.ProductDtos;
using Api.Apps.AdminApi.Dtos.ProductDtos;

namespace Api.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Brand, BrandGetAllItemDto>();

            CreateMap<Brand, BrandGetDto>();

            CreateMap<BrandDto, Brand>();

            CreateMap<Brand, Apps.ClientApi.Dtos.ProductDtos.BrandInProductGetDto>();

            CreateMap<Product, Apps.ClientApi.Dtos.ProductDtos.ProductGetAllItemDto>()
                .ForMember(d => d.DiscountedPrice, s => s.MapFrom(x => (x.SalePrice * (100 - x.DiscountPercent) / 100)));

            CreateMap<Product, Apps.ClientApi.Dtos.ProductDtos.ProductGetDto>()
                .ForMember(d => d.DiscountedPrice, s => s.MapFrom(x => (x.SalePrice * (100 - x.DiscountPercent) / 100)));

            CreateMap<Apps.AdminApi.Dtos.ProductDtos.ProductDto, Product>();
        }
    }
}
