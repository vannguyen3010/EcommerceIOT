using Entities.Models;

namespace Contracts
{
    public interface ICartRepository
    {
        Task<CartItem> GetCartItemByProductIdAndUserIdAsync(Guid productId, string userId);
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId, bool trackChanges);
        Task DeleteCartItemsByUserIdAsync(string userId);
        Task AddCartItemAsync(CartItem cartItem);
        void UpdateCartItem(CartItem cartItem);
        void DeleteCartItem(CartItem cartItem);
        Task<bool> SaveAsync();
    }
}
