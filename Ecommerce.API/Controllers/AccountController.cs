using AutoMapper;
using Contracts;
using Ecommerce.API.JwtFeatures;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Response;
using Shared.DTO.User;
using Shared.Enum;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        private readonly ILoggerManager _logger;

        public AccountController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler, ILoggerManager logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Dữ liệu đăng ký không hợp lệ!",
                    Data = null
                });
            }

            // Kiểm tra email đã tồn tại
            var existingUser = await _userManager.FindByEmailAsync(request.Email!);
            if (existingUser != null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email đã được đăng ký vui lòng nhập email khác!",
                    Data = null
                });
            }

            var user = _mapper.Map<User>(request);

            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, request.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return BadRequest(new RegisterResponseDto { Errors = errors });
            }
            //   // Thêm người dùng vào vai trò "User"
            await _userManager.AddToRoleAsync(user, "User");

            return StatusCode(201);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Kiểm tra Email và Mật khẩu!");

            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email!);
            if (user == null)
            {
                return BadRequest("Kiểm tra Email và Mật khẩu!");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password!))
            {
                return BadRequest("Kiểm tra Email và Mật khẩu!");
            }

            if (user.EmailConfirmed == false)
            {
                return BadRequest("Kiểm tra Email và Mật khẩu!");
            }

            var token = await _jwtHandler.GenerateToken(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token,  UserId = user.Id });
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

                foreach (var userDto in userDtos)
                {
                    var user = await _userManager.FindByIdAsync(userDto.Id!);
                    var roles = await _userManager.GetRolesAsync(user!);
                    userDto.Roles = roles;
                }
                return Ok(new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = userDtos
                });
            }
            catch (Exception)
            {

                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu của bạn.");
            }
        }
    }
}
