using Shared.DTO.Response;
using Shared.DTO.User;

namespace EcommerceIOT.Client.Services
{
    public class AccountServices(HttpClient httpClient)
    {
        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var result = await httpClient.PostAsJsonAsync($"/api/Account/Login", request);
            if (!result.IsSuccessStatusCode)
            {
                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                };
            }
            var response = await result.Content.ReadFromJsonAsync<AuthResponseDto>();
            return response!;
        }
    }
}
