using Shared.DTO.Order;
using Shared.DTO.Response;

namespace EcommerceIOT.Client.Services
{
    public class OrderServices
    {
        private readonly HttpClient _httpClient;

        public OrderServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Order/CreateOrder", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
            }
            else
            {
                var errorResponse = new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Failed to create order. Please try again."
                };
                return errorResponse;
            }
        }

        public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(Guid orderId)
        {
            var response = await _httpClient.GetAsync($"/api/Order/GetOrderById/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
            }

            return null;
        }

    }
}
