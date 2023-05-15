using Api.Apps.AdminApi.Dtos;
using Core.Entities;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Apps.AdminApi.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public BrandsController(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var data = _context.Brands.ToList();

            List<BrandGetAllItemDto> items = data.Select(x => new BrandGetAllItemDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = _context.Brands.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            if (data == null) return NotFound();

            BrandGetDto dto = new BrandGetDto { Id = data.Id,Name = data.Name,ProductCount=data.Products.Count };

            return Ok(dto);
        }


        [HttpPost("")]
        public IActionResult Create(BrandDto brandDto)
        {
            if (_context.Brands.Any(x => x.Name == brandDto.Name))
            {
                ModelState.AddModelError("Name", "brand already exist");
                return BadRequest(ModelState);
            }

            Brand brand = new Brand { Name = brandDto.Name };

            _context.Brands.Add(brand);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, brand);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,BrandDto brandDto)
        {
            var existData = _context.Brands.Find(id);

            if(existData == null) return NotFound();

            if(existData.Name != brandDto.Name && _context.Brands.Any(x=>x.Name == brandDto.Name))
            {
                ModelState.AddModelError("Name", "brand already exist");
                return BadRequest(ModelState);
            }

            existData.Name = brandDto.Name;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _context.Brands.Find(id);

            if (data == null) return NotFound();

            _context.Brands.Remove(data);

            _context.SaveChanges();

            return NoContent();
        }
    }
}
