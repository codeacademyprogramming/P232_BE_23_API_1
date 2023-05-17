using Api.Apps.ClientApi.Dtos.UserDtos;
using Api.Services;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Apps.ClientApi.Controllers
{
    [ApiExplorerSettings(GroupName = "user_v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtService _jwtService;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, JwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        //[HttpGet("")]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

        //    return Ok();

        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            if(_userManager.Users.Any(x=>x.Email == registerDto.Email))
            {
                ModelState.AddModelError("Email", "Email already taken!");
                return BadRequest(ModelState);
            }

            if (_userManager.Users.Any(x => x.UserName == registerDto.UserName))
            {
                ModelState.AddModelError("UserName", "UserName already taken!");
                return BadRequest(ModelState);
            }

            AppUser user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FullName = registerDto.FullName,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Password is not correct format");
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null) return NotFound();

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return BadRequest();

            var tokenStr = _jwtService.Generate(user, _userManager.GetRolesAsync(user).Result);
            
            return Ok(new {token = tokenStr});
        }

        [Authorize(Roles = "Member")]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            return Ok(user);
        }
    }
}
