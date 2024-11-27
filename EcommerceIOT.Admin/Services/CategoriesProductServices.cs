using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Product;
using Shared.DTO.Response;
using System.Net.Http.Headers;

namespace EcommerceIOT.Admin.Services
{
    public class CategoriesProductServices
    {
        private readonly HttpClient _httpClient;

        public CategoriesProductServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CateProductDto>> GetAllCategoryProducts()
        {
            var response = await _httpClient.GetAsync("/api/CateProduct/GetAllCategoryProducts");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CateProductDto>>>();
                return apiResponse?.Data ?? Enumerable.Empty<CateProductDto>();
            }
            else
            {
                return Enumerable.Empty<CateProductDto>();
            }
        }
        public async Task<bool> DeleteCategoryProductAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/CateProduct/DeleteCategoryProduct?id={id}");

            return response.IsSuccessStatusCode;

        }
        public async Task<bool> UpdateCateProductAsync(Guid id, UpdateCateProductDto request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/CateProduct/UpdateCategoryProduct/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResponse<CateProductDto>> GetCateProductByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/CateProduct/GetCategoryProductByCategoryId/{id}");

            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadFromJsonAsync<ApiResponse<CateProductDto>>();
                return category;
            }
            return null;
        }

        public async Task<bool> CreateCategoriesAsync(CreateCateProductDto request)
        {
            // Gửi request
            var response = await _httpClient.PostAsJsonAsync("/api/CateProduct/CreateCategoryProduct", request);
            return response.IsSuccessStatusCode;
        }
    }
}
