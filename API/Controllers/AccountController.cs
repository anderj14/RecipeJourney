
using System.Security.Claims;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helper;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ITokenService tokenService, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected async Task<AppUser> GetAuthenticatedUserAsync()
        {
            var email = User.GetEmail();

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }

        [HttpGet("emailexists")]
        [Authorize]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400));
            }

            try
            {
                if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                {
                    return BadRequest(new { Errors = new[] { "Email address is in use: ", registerDto.Email } });
                }

                var appUser = new AppUser
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };

                var result = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (!result.Succeeded) return BadRequest(new ApiResponse(400, useSeriousMessages: true));

                var roleAddResult = await _userManager.AddToRoleAsync(appUser, "MEMBER");

                if (!roleAddResult.Succeeded) return BadRequest(new ApiResponse(400, "Failed to add to role"));

                return new UserDto
                {
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = await _tokenService.CreateTokenAsync(appUser)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400));
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null | !await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized(new ApiResponse(401));

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

                return new UserDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.CreateTokenAsync(user)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("allusers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetUsers([FromQuery] UserSpecParams userSpecParams)
        {
            var totalItems = await _userManager.Users.CountAsync();

            if (totalItems == 0) return Ok(new PagedList<UserDto>(new List<UserDto>(), 0, userSpecParams.PageIndex, userSpecParams.PageSize));

            var users = await _userManager.Users
            .Skip((userSpecParams.PageIndex - 1) * userSpecParams.PageSize)
            .Take(userSpecParams.PageSize)
            .ToListAsync();

            var data = _mapper.Map<IReadOnlyList<UserDto>>(users);

            var paginatedUsers = new PagedList<UserDto>(
                data.ToList(),
                totalItems,
                userSpecParams.PageIndex,
                userSpecParams.PageSize
            );

            Response.AddPaginationHeader(paginatedUsers.MetaData);

            return Ok(paginatedUsers);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            var user = await GetAuthenticatedUserAsync();

            if (user == null)
                return Unauthorized(new ApiResponse(401, "User not authenticated or not found"));

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                Roles = User.FindFirstValue(ClaimTypes.Role)
            });

        }
    }
}