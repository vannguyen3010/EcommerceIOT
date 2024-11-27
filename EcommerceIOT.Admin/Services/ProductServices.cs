using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Shared.DTO.Introduce;
using Shared.DTO.Product;
using Shared.DTO.Response;
using System.Net.Http.Headers;

namespace EcommerceIOT.Admin.Services
{
    public class ProductServices
    {
        private readonly HttpClient _httpClient;

        public ProductServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<ProductResponseDto>> GetListProductAsync(int pageNumber = 1, int pageSize = 10, decimal? minPrice = null, decimal? maxPrice = null, Guid? categoryId = null, string? keyword = null, int? type = null)
        {
            // Tạo URL gốc cho API
            var queryParameters = new Dictionary<string, string>
            {
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"] = pageSize.ToString()
            };


            if (categoryId.HasValue)
            {
                queryParameters["categoryId"] = categoryId.Value.ToString();
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                queryParameters["keyword"] = keyword;
            }


            // Sử dụng QueryHelpers để tạo chuỗi truy vấn
            var query = QueryHelpers.AddQueryString("/api/Product/GetListProduct", queryParameters);

            var response = await _httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductResponseDto>>();
                return result;
            }

            return null;
        }
        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Product/DeleteProductById/{id}");

            return response.IsSuccessStatusCode;

        }
        public async Task<bool> CreateProduct(CreateProductDto request, IBrowserFile? file)
        {
            var content = new MultipartFormDataContent();

            // Thêm các trường khác vào form-data
            content.Add(new StringContent(request.Name), "Name");
            content.Add(new StringContent(request.Description), "Description");
            content.Add(new StringContent(request.Detail), "Detail");
            content.Add(new StringContent(request.Price.ToString()), "Price");
            content.Add(new StringContent(request.Discount.ToString()), "Discount");
            content.Add(new StringContent(request.Rating.ToString()), "Rating");
            content.Add(new StringContent(request.IsHot.ToString()), "IsHot");
            content.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");

            // Thêm ảnh chính
            if (file != null)
            {
                var fileContent = new StreamContent(file.OpenReadStream(10485760)); // 10MB limit
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "ImageFile", file.Name);
            }

            // Gửi request
            var response = await _httpClient.PostAsync("/api/Product/CreateProduct", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiProductResponse<ProductDto, object>> GetProductByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/Product/GetProductById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadFromJsonAsync<ApiProductResponse<ProductDto, object>>();
                return category;
            }
            return null;
        }

        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDto request, IBrowserFile? file)
        {
            var content = new MultipartFormDataContent();

            // Thêm các trường khác vào form-data
            content.Add(new StringContent(request.Name ?? ""), "Name");
            content.Add(new StringContent(request.Description ?? ""), "Description");
            content.Add(new StringContent(request.Detail ?? ""), "Detail");
            content.Add(new StringContent(request.Price.ToString() ?? ""), "Price");
            content.Add(new StringContent(request.Discount.ToString() ?? ""), "Discount");
            content.Add(new StringContent(request.Rating.ToString()), "Rating");
            content.Add(new StringContent(request.IsHot.ToString()), "IsHot");

            // Đọc và thêm file vào form-data
            if (file != null)
            {
                var fileContent = new StreamContent(file.OpenReadStream(10485760)); // 10MB limit
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "File", file.Name);
            }

            var response = await _httpClient.PutAsync($"/api/Product/UpdateProduct/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResponse<ProductResponseDto>> GetAllProductsPagination(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/api/Product/GetAllProductsPagination?pageNumber={pageNumber}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ApiResponse<ProductResponseDto>>();
                return data!;
            }

            return new ApiResponse<ProductResponseDto>();
        }
    }
}
