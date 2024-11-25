using Shared.DTO.Response;
using Shared.DTO.User;
using System.Net.Http.Json;

namespace Ecommerce.Client.Services
{
    public class AccountServices
    {
        private readonly HttpClient _httpClient;

        public AccountServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Register", registerDto);
            if (response.IsSuccessStatusCode)
            {
                return new RegisterResponseDto { IsSuccessfulRegistration = true };
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
            return new RegisterResponseDto
            {
                IsSuccessfulRegistration = false,
                Errors = errorResponse?.Errors,
                Message = errorResponse?.Message
            };

        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/Account/Login", request);
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
