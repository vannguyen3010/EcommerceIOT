using AutoMapper;
using Contracts;
using Ecommerce.API.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Shared.DTO.Product;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly RepositoryContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ILoggerManager logger, IRepositoryManager repository, RepositoryContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _repository = repository;
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            ValidateFileUpload(createProductDto);
            try
            {
                // Kiểm tra nếu tên sản phẩm đã tồn tại hay chưa
                var existingProduct = await _repository.Product.GetProductByNameAsync(createProductDto.Name!);
                if (existingProduct != null)
                {
                    _logger.LogError($"Name with name '{existingProduct.Name}' already exists.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Name with name '{existingProduct.Name}' already exists.",
                        Data = null
                    });
                }

                //Kiểm id Danh muc có hợp lệ ko 
                var category = await _repository.CateProduct.GetCategoryProductByIdAsync(createProductDto.CategoryId, trackChanges: false);
                if (category == null)
                {
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Invalid category ID.",
                        Data = null
                    });
                }

                // Kiểm tra Discount lớn hơn Price hay không
                if (createProductDto.Discount > createProductDto.Price)
                {
                    _logger.LogError("Sản phẩm giảm giá không được lớn hơn giá gốc.");
                    return BadRequest(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = "Sản phẩm giảm giá không được lớn hơn giá gốc.",
                        Data = null
                    });
                }


                var productEntity = _mapper.Map<Product>(createProductDto);

                // Lấy CategoryName từ danh mục
                productEntity.CategoryName = category.Name;

                // Tạo NameSlug từ Title
                productEntity.NameSlug = SlugGenerator.GenerateSlug(createProductDto.Name);


                //Xử lý hình ảnh
                if (createProductDto.ImageFile != null)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(createProductDto.ImageFile.FileName)}";
                    var fileExtension = Path.GetExtension(createProductDto.ImageFile.FileName);
                    productEntity.ImageFilePath = await SaveFileAndGetUrl(createProductDto.ImageFile, fileName, fileExtension);
                    productEntity.ImageFileName = fileName;
                    productEntity.ImageFileExtension = fileExtension;
                    productEntity.ImageFileSizeInBytes = createProductDto.ImageFile.Length;
                }

                //Save product to db
                await _repository.Product.CreateProductAsync(productEntity);

                return CreatedAtAction(nameof(CreateProduct), new ApiResponse<ProductDto>
                {
                    Success = true,
                    Message = "Product created successfully.",
                    Data = _mapper.Map<ProductDto>(productEntity)
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside UpdateBrand action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetListProduct")]
        public async Task<IActionResult> GetListProduct([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null, [FromQuery] Guid? categoryId = null, string? keyword = null, int? type = null)
        {
            try
            {
                var (products, totalCount) = await _repository.Product.GetListProducAsync(pageNumber, pageSize, minPrice, maxPrice, categoryId, keyword, type);

                if (!products.Any())
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No products found.",
                        Data = null
                    });
                }

                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);


                return Ok(new
                {
                    success = true,
                    message = "Products retrieved successfully.",
                    data = new
                    {
                        totalCount,
                        products = productDtos
                    }
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetAllProductsPagination action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _repository.Product.GetProductByIdAsync(id, trackChanges: false);

                if (product == null)
                {
                    _logger.LogError($"Product with id {id} not found.");
                    return NotFound(new ApiProductResponse<Object, Object>
                    {
                        Success = false,
                        Message = $"Product with id {id} not found.",
                        Data = null,
                        Data2nd = null
                    });
                }

                var category = await _repository.CateProduct.GetCategoryProductByIdAsync(product.CategoryId, trackChanges: false);

                if (category == null)
                {
                    _logger.LogError($"Category for product with id {id} not found.");
                    return NotFound(new ApiProductResponse<Object, Object>
                    {
                        Success = false,
                        Message = $"Category for product with id {id} not found.",
                        Data = null,
                        Data2nd = null
                    });
                }


                //Ánh xạ sản phẩm sang DTO để trả về client
                var productDto = _mapper.Map<ProductDto>(product);


                // Lấy danh sách sản phẩm liên quan (ví dụ: cùng danh mục)
                var relatedProducts = await _repository.Product.GetRelatedProductsAsync(id, product.CategoryId, trackChanges: true);

                var relatedProductsDto = _mapper.Map<IEnumerable<ProductDto>>(relatedProducts);

                // Tạo phản hồi và lưu vào cache
                return Ok(new ApiProductResponse<ProductDto, IEnumerable<ProductDto>>
                {
                    Success = true,
                    Message = "Product retrieved successfully.",
                    Data = productDto,
                    Data2nd = relatedProductsDto
                });

            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetProductById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteProductById/{id}")]
        public async Task<IActionResult> DeleteProductById(Guid id)
        {
            try
            {
                //kiểm tra sản phẩm có tồn tại không
                var product = await _repository.Product.GetProductByIdAsync(id, trackChanges: false);
                if (product == null)
                {
                    _logger.LogError($"Product with id {id} not found.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Product with id {id} not found.",
                        Data = null
                    });
                }

                //Xóa Sản phẩm
                await _repository.Product.DeleteProductAsync(product);
                return Ok(new ApiResponse<Object>
                {
                    Success = true,
                    Message = "Product deleted successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside DeleteProductById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                UpdateFileUpload(updateProductDto);

                //Kiểm tra sản phẩm có tồn tại không
                var productEntity = await _repository.Product.GetProductByIdAsync(id, trackChanges: true);

                if (productEntity == null)
                {
                    _logger.LogError($"Không tìm thấy sản phẩm có id {id}");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Không tìm thấy sản phẩm có id {id}",
                        Data = null
                    });
                }

                // Kiểm tra Discount lớn hơn Price không
                if (updateProductDto.Discount > updateProductDto.Price)
                {
                    _logger.LogError("Discount cannot be greater than price.");
                    return BadRequest(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = "Discount cannot be greater than price.",
                        Data = null
                    });
                }

                // Kiểm tra nếu tên sản phẩm đã tồn tại hay chưa
                var existingProduct = await _repository.Product.GetProductByNameAsync(updateProductDto.Name!);
                if (existingProduct != null)
                {
                    _logger.LogError($"Name with name '{existingProduct.Name}' already exists.");
                    return NotFound(new ApiResponse<Object>
                    {
                        Success = false,
                        Message = $"Name with name '{existingProduct.Name}' already exists.",
                        Data = null
                    });
                }

                //Cập nhật các thông tin sản phẩm
                _mapper.Map(updateProductDto, productEntity);
                productEntity.NameSlug = SlugGenerator.GenerateSlug(updateProductDto.Name);
                productEntity.UpdatedAt = DateTime.UtcNow;

                // Nếu có file ảnh mới, cập nhật file if (updateProductDto.ImageFile != null)
                if (updateProductDto.File != null)
                {

                    string mainImageFileName = $"{Guid.NewGuid()}{Path.GetExtension(updateProductDto.File.FileName)}";
                    productEntity.ImageFilePath = await SaveFileAndGetUrl(updateProductDto.File, mainImageFileName, Path.GetExtension(updateProductDto.File.FileName));
                    productEntity.ImageFileName = mainImageFileName;
                    productEntity.ImageFileExtension = Path.GetExtension(updateProductDto.File.FileName);
                    productEntity.ImageFileSizeInBytes = updateProductDto.File.Length;
                }

                await _repository.Product.UpdateProductAsync(productEntity);

                return Ok(new ApiResponse<ProductDto>
                {
                    Success = true,
                    Message = "Sản phẩm được cập nhật thành công.",
                    Data = _mapper.Map<ProductDto>(productEntity)
                });

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError($"Concurrency conflict occurred: {ex.Message}");
                return Conflict(new ApiResponse<Object>
                {
                    Success = false,
                    Message = "Concurrency conflict occurred. Please try again.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetAllProductsPagination")]
        public async Task<IActionResult> GetAllProductsPagination([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                // Gọi repository để lấy sản phẩm với phân trang
                var (products, totalCount) = await _repository.Product.GetAllProductPaginationAsync(pageNumber, pageSize);

                if (!products.Any())
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No products found.",
                        Data = null
                    });
                }

                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

                // Trả về response với data và số lượng sản phẩm
                return Ok(new
                {
                    success = true,
                    message = "Products retrieved successfully.",
                    data = new
                    {
                        totalCount,
                        products = productDtos
                    }
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside GetAllProducts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private void UpdateFileUpload(UpdateProductDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (request.File != null)
            {
                ////// Kiểm tra phần mở rộng tệp
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
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Img_Repository/Product", $"{fileName}{fileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Img_Repository/Product/{fileName}{fileExtension}";

            return urlFilePath;
        }
        private void ValidateFileUpload(CreateProductDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (request.ImageFile != null)
            {
                //// Kiểm tra phần mở rộng tệp
                if (allowedExtensions.Contains(Path.GetExtension(request.ImageFile.FileName)) == false)
                {
                    ModelState.AddModelError("File", "Unsupported file extension");
                }

                //// Kiểm tra kích thước tệp
                if (request.ImageFile.Length > 10485760)// Tệp lớn hơn 10MB
                {
                    ModelState.AddModelError("File", "file size more than 10MB, please upload a smaller size file .");
                }
            }

        }
    }
}
