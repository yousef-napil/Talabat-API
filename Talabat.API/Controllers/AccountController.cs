using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs.Identity;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager ,
                                 SignInManager<AppUser> signInManager ,
                                 ITokenService tokenService ,
                                 IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email Already Exists"));
            }
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

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var mappedUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            };
            return Ok(mappedUser);
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.GetUserWithAddressAsync(User);
            var mappedAddress = mapper.Map<AddressDto>(user?.Address);
            if (mappedAddress == null) return NotFound(new ApiResponse(404));
            return Ok(mappedAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await userManager.GetUserWithAddressAsync(User);
            address.Id = user.Address.Id;
            var UserAddress = mapper.Map<Address>(address);
            user.Address = UserAddress;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to update address"));
            return Ok(address);
        }
        [Authorize]
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }

    }
}
