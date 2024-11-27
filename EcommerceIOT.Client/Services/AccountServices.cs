using Blazored.LocalStorage;
using Shared.DTO.Response;
using Shared.DTO.User;
using System.Net.Http;

namespace EcommerceIOT.Client.Services
{
    public class AccountServices(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        public async Task<AuthResponseDto> Login(LoginDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/Account/Login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                // Lưu UserId vào LocalStorage
                if (result != null && result.IsAuthSuccessful)
                {
                    await localStorageService.SetItemAsync("userId", result.UserId);
                    await localStorageService.SetItemAsync("authToken", result.Token);

                }
                return result ?? new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "No data received" };
            }
            else
            {
                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = $"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}"
                };
            }
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            var response = await httpClient.PostAsJsonAsync("/api/Account/Register", registerDto);
            if (response.IsSuccessStatusCode)
            {
                return new AuthResponseDto { IsAuthSuccessful = true };
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            return new AuthResponseDto
            {
                IsAuthSuccessful = false
            };

        }
    }

}
