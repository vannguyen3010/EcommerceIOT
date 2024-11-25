using Microsoft.AspNetCore.WebUtilities;
using Shared.DTO.Response;

namespace EcommerceIOT.Admin.Services
{
    public class ContactServices
    {
        private readonly HttpClient _httpClient;

        public ContactServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<ContactResponse>> GetAllContactAsync(int pageNumber, int pageSize, int? type = null)
        {
            var queryParameters = new Dictionary<string, string>
            {
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"] = pageSize.ToString()
            };

            if (type.HasValue)
            {
                queryParameters["type"] = type.Value.ToString();
            }

            var query = QueryHelpers.AddQueryString("/api/Contact/GetAllContact", queryParameters);

            var response = await _httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ContactResponse>>();
                return result;
            }
            return null;

        }
        public async Task<bool> DeleteContactAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Contact/DeleteContact/{id}");

            return response.IsSuccessStatusCode;

        }
    }
}
