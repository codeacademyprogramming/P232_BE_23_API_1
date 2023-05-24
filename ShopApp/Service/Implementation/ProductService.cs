using AutoMapper;
using Core.Entities;
using Core.Repositories;
using Service.Dtos.Common;
using Service.Dtos.ProductDtos;
using Service.Exceptions;
using Service.Helpers;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IBrandRepository brandRepository,IProductRepository productRepository,IMapper mapper)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<CreateResponseDto> CreteAsync(ProductDto productDto)
        {
            if (await _productRepository.IsExistAsync(x => x.Name == productDto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name already taken");

            if (!await _brandRepository.IsExistAsync(x => x.Id == productDto.BrandId))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "BrandId", "BrandId is not correct");

            Product product = _mapper.Map<Product>(productDto);

            string path = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/uploads");
            product.ImageName = FileManager.Save(productDto.File, Directory.GetCurrentDirectory() + "/wwwroot","uploads");
            product.ImageUrl = path + "/" + product.ImageName;


            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return new CreateResponseDto(product.Id);
        }

        public async Task EditAsync(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetAsync(x => x.Id == id);
            if (product == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");

            if (product.Name != productDto.Name && await _productRepository.IsExistAsync(x => x.Name == productDto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name already taken");

            if (product.BrandId != productDto.BrandId && !await _brandRepository.IsExistAsync(x => x.Id == productDto.BrandId))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "BrandId", "BrandId is not correct");

            product.Name = productDto.Name;
            product.SalePrice = productDto.SalePrice;
            product.DiscountPercent = productDto.DiscountPercent;
            product.CostPrice = productDto.CostPrice;
            product.BrandId = productDto.BrandId;

            await _productRepository.SaveChangesAsync();
        }

        public async Task<List<ProductGetAllItemDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync("Brand");

            var data = _mapper.Map<List<ProductGetAllItemDto>>(products);

            return data;
        }

        public async Task<ProductGetDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetAsync(x => x.Id == id, "Brand");

            if (product == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");

            var productDto = _mapper.Map<ProductGetDto>(product);
            return productDto;
        }
    }
}
