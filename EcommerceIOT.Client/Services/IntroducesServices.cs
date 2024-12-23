﻿using Microsoft.AspNetCore.WebUtilities;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Introduce;
using Shared.DTO.Response;

namespace EcommerceIOT.Client.Services
{
    public class IntroducesServices
    {
        private readonly HttpClient _httpClient;

        public IntroducesServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<IntroduceResponse>> GetListIntroduceAsync(int pageNumber = 1, int pageSize = 10, Guid? categoryId = null, string? keyword = null, int? type = null)
        {
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

            if (type.HasValue)
            {
                queryParameters["type"] = type.Value.ToString();  // Thêm tham số type vào chuỗi truy vấn
            }

            var query = QueryHelpers.AddQueryString("/api/Introduce/GetListIntroduce", queryParameters);

            var response = await _httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IntroduceResponse>>();
                return result;
            }
            return null;
        }
        public async Task<ApiResponse<CategoryIntroduceDto>> GetIntroduceByCategoryId(Guid Id)
        {
            var response = await _httpClient.GetAsync($"/api/CategoryIntroduce/GetCateIntroduceByCategoryId/{Id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse<CategoryIntroduceDto>>();
            }
            return null;
        }
        public async Task<ApiProductResponse<IntroduceDto, IEnumerable<IntroduceDto>>> GetIntroduceByIdAsync(Guid Id)
        {
            var response = await _httpClient.GetAsync($"/api/Introduce/GetIntroduceById/{Id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiProductResponse<IntroduceDto, IEnumerable<IntroduceDto>>>();
            }
            return null;
        }

        public async Task<IEnumerable<CategoryIntroduceDto>> GetAllCategoryIntroduces()
        {
            var response = await _httpClient.GetAsync("/api/CategoryIntroduce/GetAllCategory");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CategoryIntroduceDto>>>();
                return apiResponse?.Data ?? Enumerable.Empty<CategoryIntroduceDto>();
            }
            else
            {
                return Enumerable.Empty<CategoryIntroduceDto>();
            }
        }
    }
}
