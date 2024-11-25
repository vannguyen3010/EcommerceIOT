using AutoMapper;
using Contracts;
using Ecommerce.API.JwtFeatures;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Response;
using Shared.DTO.User;

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
        [AllowAnonymous]
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
    }
}
