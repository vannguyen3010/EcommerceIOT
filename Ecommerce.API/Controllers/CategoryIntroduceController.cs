using AutoMapper;
using Contracts;
using Ecommerce.API.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryIntroduceController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CategoryIntroduceController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryIntroDto request)
        {
            try
            {
                //Kiểm tra tên danh muc đã có chưa
                var existingCategory = await _repository.CateIntroduce.GetCategoryIntroduceByNameAsync(request.Name!);
                if (existingCategory != null)
                {
                    _logger.LogError($"Name with name '{existingCategory.Name}' already exists.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Name with name '{existingCategory.Name}' already exists.",
                        Data = null
                    });
                }

                var categoryEntity = _mapper.Map<CategoryNew>(request);

                categoryEntity.Status = true;

                // Tạo NameSlug từ Title
                categoryEntity.NameSlug = SlugGenerator.GenerateSlug(request.Name);

                // Lưu danh mục vào cơ sở dữ liệu
                await _repository.CateIntroduce.CreateCategoryIntroduceAsync(categoryEntity, trackChanges: false);

                return Ok(new ApiResponse<CategoryIntroduceDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = _mapper.Map<CategoryIntroduceDto>(categoryEntity)
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside CreateCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetCateIntroduceByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetCateIntroduceByCategoryId(Guid categoryId)
        {
            try
            {
                // Lấy category cấp 1 theo id
                var categoryEntity = await _repository.CateIntroduce.GetCategoryIntroduceByIdAsync(categoryId, trackChanges: false);
                if (categoryEntity == null)
                {
                    _logger.LogError($"Category with id: {categoryId} not found.");
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Category with id: {categoryId} not found.",
                        Data = null
                    });
                }

                //Chuyển đổi dữ liệu sang DTO
                var categoryIntrolduceDto = _mapper.Map<CategoryIntroduceDto>(categoryEntity);

                return Ok(new ApiResponse<CategoryIntroduceDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = categoryIntrolduceDto
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetCategoryById action: {ex.Message}");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Internal server error",
                    Data = null
                });
            }
        }

        [HttpGet]
        [Route("GetAllCategoryPagination")]
        public async Task<IActionResult> GetAllCategoryPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        {
            try
            {
                var (categories, totalCount) = await _repository.CateIntroduce.GetAllCategoryIntroducePagitionAsync(pageNumber, pageSize, keyword);

                if (!categories.Any())
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Không có danh mục nào!",
                        Data = null
                    });
                }

                var categoryDtos = _mapper.Map<IEnumerable<CategoryIntroduceDto>>(categories);

                return Ok(new
                {
                    success = true,
                    message = "Products retrieved successfully.",
                    data = new
                    {
                        totalCount,
                        categories = categoryDtos
                    }
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside CreateCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateCategoryIntroduce/{Id}")]
        public async Task<IActionResult> UpdateCategoryIntroduce(Guid Id, [FromBody] UpdateCateIntroDto introduce)
        {
            try
            {
                if (introduce == null)
                {
                    _logger.LogError($"Không tìm thấy {Id} danh mục này!");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy {Id} danh mục này!",
                        Data = null
                    });
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Đối tượng danh mục không hợp lệ.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Đối tượng danh mục không hợp lệ",
                        Data = null
                    });
                }

                var categoryEntity = await _repository.CateIntroduce.GetCategoryIntroduceByIdAsync(Id, trackChanges: true);
                if (categoryEntity == null)
                {
                    _logger.LogError($"Danh mục có id: {Id}, không được tìm thấy trong db.");
                    return NotFound();
                }
                categoryEntity.Name = introduce.Name;
                categoryEntity.NameSlug = SlugGenerator.GenerateSlug(introduce.Name);

                _repository.CateIntroduce.UpdateCategory(categoryEntity);
                _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside UpdateCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("DeleteCategoryIntroduce/{Id}")]
        public async Task<IActionResult> DeleteCategoryIntroduce(Guid Id)
        {
            try
            {
                var category = await _repository.CateIntroduce.GetCategoryIntroduceByIdAsync(Id, trackChanges: false);
                if (category == null)
                {
                    _logger.LogError($"Không tìm thấy {Id} danh mục này!");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy {Id} danh mục này!",
                        Data = null
                    });
                }


                _repository.CateIntroduce.DeleteCategory(category);
                _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
