using AutoMapper;
using Contracts;
using Ecommerce.API.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Introduce;
using Shared.DTO.Product;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CateProductController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CateProductController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateCategoryProduct")]
        public async Task<IActionResult> CreateCategoryProduct([FromForm] CreateCateProductDto createCategoryDto)
        {
            try
            {
                // Kiểm tra nếu tên danh mục đã tồn tại
                var existingCategory = await _repository.CateProduct.GetCategoryByNameAsync(createCategoryDto.Name!);
                if (existingCategory != null)
                {
                    _logger.LogError($"Category with name '{createCategoryDto.Name}' already exists.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Category with name '{createCategoryDto.Name}' already exists.",
                        Data = null
                    });
                }
                // Ánh xạ Dto thành entity
                var cateProductEntity = _mapper.Map<CateProduct>(createCategoryDto);

                // Tạo NameSlug từ Title
                cateProductEntity.NameSlug = SlugGenerator.GenerateSlug(createCategoryDto.Name);

                // Lưu danh mục vào cơ sở dữ liệu
                await _repository.CateProduct.CreateCategoryAsync(cateProductEntity);

                return Ok(new ApiResponse<CateProductDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = _mapper.Map<CateProductDto>(cateProductEntity)
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside CreateCateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetAllCategoryProducts")]
        public async Task<IActionResult> GetAllCategoryProducts()
        {
            try
            {
                var categories = await _repository.CateProduct.GetAllCategoryProduct(trackChanges: false);
                if (categories == null)
                {
                    _logger.LogError("Không có danh mục nào!");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không có danh mục nào!",
                        Data = null
                    });
                }


                var categoryDtos = _mapper.Map<IEnumerable<CateProductDto>>(categories);

                return Ok(new ApiResponse<IEnumerable<CateProductDto>>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = categoryDtos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllCategories action: {ex.Message}");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Internal server error",
                    Data = null
                });
            }
        }

        [HttpPut]
        [Route("UpdateCategoryProduct/{id}")]
        public async Task<IActionResult> UpdateCategoryProduct(Guid id, [FromForm] UpdateCateProductDto updateCateProduct)
        {
            try
            {
                var existingCateProduct = await _repository.CateProduct.GetCategoryProductByIdAsync(id, trackChanges: false);
                if (existingCateProduct == null)
                {
                    _logger.LogError($"Category with id {id} not found.");
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Category with id {id} not found.",
                        Data = null
                    });

                }

                // Update category properties
                existingCateProduct.Name = updateCateProduct.Name;

                existingCateProduct.DateTime = DateTime.UtcNow; // Update the DateTime field

                await _repository.CateProduct.UpdateCategoryAsync(existingCateProduct);
                return Ok(new ApiResponse<CateProductDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = _mapper.Map<CateProductDto>(existingCateProduct)
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside DeleteCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteCategoryProduct")]
        public async Task<IActionResult> DeleteCategoryProduct(Guid id)
        {
            try
            {
                // Kiểm tra xem danh mục có sản phẩm không
                var hasProducts = await _repository.CateProduct.HasProductsInCategoryAsync(id);

                if (hasProducts)
                {
                    _logger.LogError($"Không thể xóa danh mục có id {id} vì nó chứa sản phẩm.");
                    return BadRequest(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không thể xóa danh mục có id {id} vì nó chứa sản phẩm.",
                        Data = null
                    });
                }

                // Kiểm tra xem danh mục có tồn tại không
                var category = await _repository.CateProduct.GetCategoryProductByIdAsync(id, trackChanges: false);

                if (category == null)
                {
                    _logger.LogError($"Không tìm thấy danh mục có id {id}.");
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy danh mục có id {id}.",
                        Data = null
                    });

                }


                await _repository.CateProduct.DeleteCategoryAsync(category);

                return Ok(new ApiResponse<Object>
                {
                    Success = true,
                    Message = "Category deleted successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside DeleteCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
