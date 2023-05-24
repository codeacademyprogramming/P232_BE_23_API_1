using AutoMapper;
using Core.Entities;
using Service.Dtos.BrandDtos;
using Service.Dtos.ProductDtos;

namespace Api.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Brand, BrandGetAllItemDto>();

            CreateMap<Brand, BrandGetDto>();

            CreateMap<BrandDto, Brand>();
            CreateMap<Brand, BrandCreateResponseDto>();

            CreateMap<ProductDto, Product>();

            CreateMap<Product, ProductGetAllItemDto>();
            CreateMap<Product, ProductGetDto>();
            CreateMap<Brand, BrandInProductGetDto>();

        }
    }
}
