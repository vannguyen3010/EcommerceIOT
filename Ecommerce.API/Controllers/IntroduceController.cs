using AutoMapper;
using Contracts;
using Ecommerce.API.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Introduce;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntroduceController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IntroduceController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("CreateIntroduce")]
        public async Task<IActionResult> CreateIntroduce([FromForm] CreateIntroduceDto createIntroduceDto)
        {
            try
            {
                ValidateFileUpload(createIntroduceDto);

                //Kiểm id Danh muc có hợp lệ ko 
                var category = await _repository.CateIntroduce.GetCategoryIntroduceByIdAsync(createIntroduceDto.CategoryId, trackChanges: false);
                if (category == null)
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy id này",
                        Data = null
                    });
                }


                // Kiểm tra nếu tên bài viết đã tồn tại hay chưa
                var existingIntroduce = await _repository.Introduce.GetIntroduceByNameAsync(createIntroduceDto.Name!);
                if (existingIntroduce != null)
                {
                    _logger.LogError($"Title with name '{existingIntroduce.Name}' already exists.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Title with name '{existingIntroduce.Name}' already exists.",
                        Data = null
                    });
                }

                // Ánh xạ Dto thành entity
                var introduceEntity = _mapper.Map<New>(createIntroduceDto);

                // Gán CategoryName cho Introduce entity
                introduceEntity.CategoryName = category.Name;
                introduceEntity.Status = true;
                introduceEntity.FileDescription = createIntroduceDto.Name;
                introduceEntity.FileName = createIntroduceDto.Name;

                // Tạo NameSlug từ Title
                introduceEntity.NameSlug = SlugGenerator.GenerateSlug(createIntroduceDto.Name);

                // Xử lý tập tin hình ảnh
                if (createIntroduceDto.File != null)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(createIntroduceDto.File.FileName)}";
                    var fileExtension = Path.GetExtension(createIntroduceDto.File.FileName);
                    introduceEntity.FilePath = await SaveFileAndGetUrl(createIntroduceDto.File, fileName, fileExtension);
                    introduceEntity.FileName = fileName;
                    introduceEntity.FileExtension = fileExtension;
                    introduceEntity.FileSizeInBytes = createIntroduceDto.File.Length;
                }

                // tạo danh mục vào cơ sở dữ liệu
                await _repository.Introduce.CreateIntroduceAsync(introduceEntity);

                return Ok(new ApiResponse<IntroduceDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = _mapper.Map<IntroduceDto>(introduceEntity)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Introduce action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetListIntroduce")]
        public async Task<IActionResult> GetListIntroduce([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? categoryId = null, [FromQuery] string? keyword = null, int? type = null)
        {
            try
            {
                var (introduces, totalCount) = await _repository.Introduce.GetListIntroduceAsync(pageNumber, pageSize, categoryId, keyword, type);

                if (!introduces.Any())
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Không có bài viết",
                        Data = null
                    });
                }
                var introduceDtos = _mapper.Map<IEnumerable<IntroduceDto>>(introduces);

                return Ok(new
                {
                    success = true,
                    message = "Products retrieved successfully.",
                    data = new
                    {
                        totalCount,
                        introduces = introduceDtos
                    }
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetAllIntroducesPagination action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetAllIntroduceIsHot")]
        public async Task<IActionResult> GetAllIntroduceIsHot()
        {
            try
            {
                // Lấy tất cả sản phẩm có IsHot là true
                var hotIntroduces = await _repository.Introduce.GetAllIntroduceIsHotAsync();

                if (hotIntroduces == null || !hotIntroduces.Any())
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Không tìm thấy bài viết nổi bật.",
                        Data = null
                    });
                }

                // Ánh xạ từ entity sang DTO nếu cần thiết
                var hotIntroducesDto = _mapper.Map<IEnumerable<IntroduceDto>>(hotIntroduces);

                return Ok(new ApiResponse<IEnumerable<IntroduceDto>>
                {
                    Success = true,
                    Message = "Những bài viết nổi bật.",
                    Data = hotIntroducesDto
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetAllProductIsHot action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetIntroduceById/{id}")]
        public async Task<IActionResult> GetIntroduceById(Guid id)
        {
            try
            {
                var introduce = await _repository.Introduce.GetIntroduceByIdAsync(id, trackChanges: false);
                if (introduce == null)
                {
                    _logger.LogError($"Không tìm thấy introduce id này {id}");
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Không tìm thấy introduce id này!",
                        Data = null
                    });
                }

                // Lấy danh sách bài viết liên quan (ví dụ: cùng danh mục)
                var relateIntroduces = await _repository.Introduce.GetRelatedIntroducesAsync(id, introduce.CategoryId, trackChanges: true);

                var introduceDto = _mapper.Map<IntroduceDto>(introduce);

                var introduceResult = _mapper.Map<IEnumerable<IntroduceDto>>(relateIntroduces);

                return Ok(new ApiProductResponse<IntroduceDto, IEnumerable<IntroduceDto>>
                {
                    Success = true,
                    Message = "Introduce retrieved successfully.",
                    Data = introduceDto,
                    Data2nd = introduceResult // Trả về bài viết liên quan trong Data2nd
                });

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside introduceId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateIntroduce/{id}")]
        public async Task<IActionResult> UpdateIntroduce(Guid id, [FromForm] UpdateIntroduceDto updateIntroduceDto)
        {
            try
            {
                var introduceEntity = await _repository.Introduce.GetIntroduceByIdAsync(id, trackChanges: true);
                if (introduceEntity == null)
                {
                    _logger.LogError($"Introduce with id: {id}, hasn't been found in db.");
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Introduce with id: {id}, hasn't been found in db!",
                        Data = null
                    });
                }

                var category = await _repository.CateIntroduce.GetCategoryIntroduceByIdAsync(updateIntroduceDto.CategoryId, trackChanges: false);
                if (category == null)
                {
                    _logger.LogError($"Introduce category with id: {id}, hasn't been found in db.");
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Introduce category with id: {id}, hasn't been found in db!",
                        Data = null
                    });
                }

                _mapper.Map(updateIntroduceDto, introduceEntity);

                introduceEntity.NameSlug = SlugGenerator.GenerateSlug(updateIntroduceDto.Name);
                introduceEntity.UpdatedAt = DateTime.UtcNow;
                introduceEntity.CategoryName = category.Name;
                introduceEntity.CategoryId = category.Id;

                if (updateIntroduceDto.File != null)
                {
                    string mainImageFileName = $"{Guid.NewGuid()}{Path.GetExtension(updateIntroduceDto.File.FileName)}";
                    introduceEntity.FilePath = await SaveFileAndGetUrl(updateIntroduceDto.File, mainImageFileName, Path.GetExtension(updateIntroduceDto.File.FileName));
                    introduceEntity.FileName = mainImageFileName;
                    introduceEntity.FileExtension = Path.GetExtension(updateIntroduceDto.File.FileName);
                    introduceEntity.FileSizeInBytes = updateIntroduceDto.File.Length;
                }

                _repository.Introduce.UpdateIntroduce(introduceEntity);
                _repository.SaveAsync();


                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside UpdateIntroduce action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteIntroduce/{id}")]
        public async Task<IActionResult> DeleteIntroduce(Guid id)
        {
            try
            {
                var introduce = await _repository.Introduce.GetIntroduceByIdAsync(id, trackChanges: false);
                if (introduce == null)
                {
                    _logger.LogError($"Introduce with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Introduce.DeleteIntroduce(introduce);
                _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside DeleteIntroduce action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private void ValidateFileUpload(CreateIntroduceDto request)
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
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Img_Repository/Introduce", $"{fileName}{fileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Img_Repository/Introduce/{fileName}{fileExtension}";

            return urlFilePath;
        }
    }
}
