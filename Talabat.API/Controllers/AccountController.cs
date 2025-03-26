using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs.Identity;
using Talabat.API.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;

        public AccountController(UserManager<AppUser> userManager ,
                                 SignInManager<AppUser> signInManager ,
                                 ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var User = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.PhoneNumber,
            };
            var result = await userManager.CreateAsync(User, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to register"));
            return Ok(new UserDto
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await tokenService.CreateTokenAsync(User, userManager)
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) return Unauthorized(new ApiResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(User, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await tokenService.CreateTokenAsync(User, userManager)
            });

        }

    }
}
