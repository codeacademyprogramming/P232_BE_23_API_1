using Api.Apps.AdminApi.Dtos;
using Api.Services;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Apps.AdminApi.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtService _jwtService;

        public AuthController(UserManager<AppUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpGet("createadmin")]
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser appUser = new AppUser
            {
                UserName = "admin",
                FullName = "Hikmet Abbasov",
                Email = "hiko@code.edu.az",
                IsAdmin = true,
            };

            await _userManager.CreateAsync(appUser, "Admin@123");
            await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null || !user.IsAdmin)
                return NotFound();

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return BadRequest();

            var token = _jwtService.Generate(user, await _userManager.GetRolesAsync(user));

            return Ok(new { token = token });
        }
    }
}
