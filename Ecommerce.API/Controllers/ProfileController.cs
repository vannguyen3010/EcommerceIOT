using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Profile;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("CreateSocialMediaInfo")]
        public async Task<IActionResult> CreateSocialMediaInfo([FromForm] CreateProfileDto socialMediaInfoDto)
        {
            try
            {
                ValidateFileUpload(socialMediaInfoDto);

                if (socialMediaInfoDto == null || !ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = "Dữ liệu không hợp lệ.",
                        Data = null
                    });
                }

                // Ánh xạ Dto thành entity
                var socialInfoEntity = _mapper.Map<ProfileInfo>(socialMediaInfoDto);


                // Xử lý tập tin hình ảnh
                if (socialMediaInfoDto.File != null)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(socialMediaInfoDto.File.FileName)}";
                    var fileExtension = Path.GetExtension(socialMediaInfoDto.File.FileName);
                    socialInfoEntity.FilePath = await SaveFileAndGetUrl(socialMediaInfoDto.File, fileName, fileExtension);
                    socialInfoEntity.FileName = fileName;
                    socialInfoEntity.FileExtension = fileExtension;
                    socialInfoEntity.FileSizeInBytes = socialMediaInfoDto.File.Length;
                }

                // tạo danh mục vào cơ sở dữ liệu
                await _repository.Profile.CreateSocialMediaInfoAsync(socialInfoEntity);

                return Ok(new ApiResponse<ProfileInfoDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = _mapper.Map<ProfileInfoDto>(socialInfoEntity)
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside Banner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetSocialMediaInfo/{id}")]
        public async Task<IActionResult> GetSocialMediaInfo(Guid id)
        {
            try
            {
                var profile = await _repository.Profile.GetSocialMediaInfoAsync(id, trackChanges: false);
                if (profile == null)
                {
                    _logger.LogError($"Banner with id: {id}, hasn't been found in db.");
                    return NotFound(new ApiResponse<ProfileInfoDto>
                    {
                        Success = false,
                        Message = "Banner not found.",
                        Data = null
                    });
                }

                // Chuyển đổi banner thành BannerDto
                var profileDto = _mapper.Map<ProfileInfoDto>(profile);


                return Ok(new ApiResponse<ProfileInfoDto>
                {
                    Success = true,
                    Message = "Banner retrieved successfully.",
                    Data = profileDto
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetBrandById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut]
        [Route("UpdateSocialMediaInfo/{Id}")]
        public async Task<IActionResult> UpdateSocialMediaInfo(Guid Id, [FromForm] UpdateProfile socialMediaInfoDto)
        {
            UpdateFileUpload(socialMediaInfoDto);
            try
            {
                var existingInfo = await _repository.Profile.GetSocialMediaInfoAsync(Id, trackChanges: false);
                if (existingInfo == null)
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy thông tin mạng xã hội với ID {Id}.",
                        Data = null
                    });
                }

                _mapper.Map(socialMediaInfoDto, existingInfo);

                if (socialMediaInfoDto.File != null)
                {
                    existingInfo.File = socialMediaInfoDto.File;
                    existingInfo.FileExtension = Path.GetExtension(socialMediaInfoDto.File.FileName);
                    existingInfo.FileSizeInBytes = socialMediaInfoDto.File.Length;
                    existingInfo.FileName = socialMediaInfoDto.File.FileName;
                    existingInfo.FilePath = await SaveFileAndGetUrl(socialMediaInfoDto.File, existingInfo.FileName, existingInfo.FileExtension);
                }

                await _repository.Profile.UpdateSocialMediaInfoAsync(existingInfo);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside Banner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private void UpdateFileUpload(UpdateProfile request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (request.File != null)
            {
                //// Kiểm tra phần mở rộng tệp
                if (allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) == false)
                {
                    ModelState.AddModelError("File", "Unsupported file extension");
                }

                //// Kiểm tra kích thước tệp
                if (request.File.Length > 10485760)// Tệp lớn hơn 10MB
                {
                    ModelState.AddModelError("File", "file size more than 10MB, please upload a smaller size file .");
                }
            }

        }
        private void ValidateFileUpload(CreateProfileDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (request.File != null)
            {
                //// Kiểm tra phần mở rộng tệp
                if (allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) == false)
                {
                    ModelState.AddModelError("File", "Unsupported file extension");
                }

                //// Kiểm tra kích thước tệp
                if (request.File.Length > 10485760)// Tệp lớn hơn 10MB
                {
                    ModelState.AddModelError("File", "file size more than 10MB, please upload a smaller size file .");
                }
            }

        }
        private async Task<string> SaveFileAndGetUrl(IFormFile file, string fileName, string fileExtension)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Img_Repository/img-Profile", $"{fileName}{fileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Img_Repository/img-Profile/{fileName}{fileExtension}";

            return urlFilePath;
        }
    }
}
