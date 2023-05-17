using Api.Apps.AdminApi.Dtos.BrandDtos;
using AutoMapper;
using Core.Entities;
using Core.Repositories;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Apps.AdminApi.Controllers
{
    [ApiExplorerSettings(GroupName = "admin_v1")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;

        public BrandsController( IMapper mapper, IBrandRepository brandRepository)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _brandRepository.GetAllAsync();

            List<BrandGetAllItemDto> items = _mapper.Map<List<BrandGetAllItemDto>>(data);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _brandRepository.GetAsync(x=>x.Id == id,"Products","Orders");

            if (data == null) return NotFound();

            BrandGetDto dto = _mapper.Map<BrandGetDto>(data);

            return Ok(dto);
        }


        [HttpPost("")]
        public async Task<IActionResult> Create(BrandDto brandDto)
        {
            if (await _brandRepository.IsExistAsync(x => x.Name == brandDto.Name))
            {
                ModelState.AddModelError("Name", "brand already exist");
                return BadRequest(ModelState);
            }

            Brand brand = _mapper.Map<Brand>(brandDto);

            await _brandRepository.AddAsync(brand);
            await _brandRepository.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,BrandDto brandDto)
        {
            var existData = await _brandRepository.GetAsync(x=>x.Id == id);

            if(existData == null) return NotFound();

            if(existData.Name != brandDto.Name && await _brandRepository.IsExistAsync(x=>x.Name == brandDto.Name))
            {
                ModelState.AddModelError("Name", "brand already exist");
                return BadRequest(ModelState);
            }

            existData.Name = brandDto.Name;
            await _brandRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _brandRepository.GetAsync(x=>x.Id == id);

            if (data == null) return NotFound();

            _brandRepository.Remove(data);
            await _brandRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
