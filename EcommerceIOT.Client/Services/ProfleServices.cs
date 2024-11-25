using Microsoft.AspNetCore.Components.Forms;
using Shared.DTO.Profile;
using Shared.DTO.Response;

namespace EcommerceIOT.Client.Services
{
    public class ProfileServices
    {
        private readonly HttpClient _httpClient;

        public ProfileServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<ProfileInfoDto>> GetProfileAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Profile/GetSocialMediaInfo/c7237725-8e95-45dd-0910-08dd0d849461");

            if (response.IsSuccessStatusCode)
            {
                var social = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileInfoDto>>();
                return social;
            }
            return null;
        }

    }
}
