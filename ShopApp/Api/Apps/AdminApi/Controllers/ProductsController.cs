using AutoMapper;
using Core.Entities;
using Core.Repositories;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.ProductDtos;
using Service.Interfaces;

namespace Api.Apps.AdminApi.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiExplorerSettings(GroupName = "admin_v1")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _productService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
          
            return Ok(await _productService.GetByIdAsync(id));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductDto dto)
        {
            var response = await _productService.CreteAsync(dto);

            return StatusCode(201, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,ProductDto dto)
        {
            await _productService.EditAsync(id, dto);
            return NoContent();
        }
    }
}
