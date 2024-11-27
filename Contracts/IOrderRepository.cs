using Entities.Models;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<(IEnumerable<Order> Orders, int Total)> GetOrdersListAsync(int pageNumber, int pageSize, int? type = null, string? orderCode = null);
        Task<Order> GetOrderByIdAsync(Guid orderId, bool trackChanges);
        void DeleteOrderAsync(Order order);
        Task SaveAsync();
    }
}
