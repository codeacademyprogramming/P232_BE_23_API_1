using Service.Dtos.Common;
using Service.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        public Task<CreateResponseDto> CreteAsync(ProductDto productDto);
        public Task EditAsync(int id,ProductDto productDto);
        public Task<ProductGetDto> GetByIdAsync(int id);
        public Task<List<ProductGetAllItemDto>> GetAllAsync();
    }
}
