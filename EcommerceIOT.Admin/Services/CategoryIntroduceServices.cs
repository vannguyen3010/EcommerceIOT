﻿using Microsoft.AspNetCore.WebUtilities;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Response;

namespace EcommerceIOT.Admin.Services
{
    public class CategoryIntroduceServices
    {
        private readonly HttpClient _httpClient;

        public CategoryIntroduceServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<IntroduceCategoryResponse>> GetAllCategoryPaginationAsync(int pageNumber, int pageSize, string? keyword = null)
        {
            var queryParameters = new Dictionary<string, string>
            {
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"] = pageSize.ToString()
            };

            if (!string.IsNullOrEmpty(keyword))
            {
                queryParameters["keyword"] = keyword;
            }

            var query = QueryHelpers.AddQueryString("/api/CategoryIntroduce/GetAllCategoryPagination", queryParameters);

            var response = await _httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IntroduceCategoryResponse>>();
                return result;
            }
            return null;
        }


        public async Task<ApiResponse<CategoryIntroduceDto>> GetCategoryIntroduceByIdAsync(Guid categoryId)
        {
            var response = await _httpClient.GetAsync($"/api/CategoryIntroduce/GetCateIntroduceByCategoryId/{categoryId}");
            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryIntroduceDto>>();
                return category;
            }
            return null;
        }

        public async Task<bool> UpdateCategoryIntroduceAsync(Guid id, UpdateCateIntroDto request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/CategoryIntroduce/UpdateCategoryIntroduce/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryIntroduceAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/CategoryIntroduce/DeleteCategoryIntroduce/{id}");

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> CreateCategoriesAsync(CreateCategoryIntroDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/CategoryIntroduce/CreateCategory", request);

            return response.IsSuccessStatusCode;
        }
    }
}