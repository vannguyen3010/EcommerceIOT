using Entities.Models;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<Product> GetProductByNameAsync(string name);
        Task<Product> CreateProductAsync(Product product);
        Task<(IEnumerable<Product>, int totalCount)> GetListProducAsync(int pageNumber, int pageSize, decimal? minPrice = null, decimal? maxPrice = null, Guid? categoryId = null, string? keyword = null, int? type = null);
        Task<Product> GetProductByIdAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Product>> GetRelatedProductsAsync(Guid productId, Guid categoryId, bool trackChanges);
        Task<(IEnumerable<Product> Products, int Total)> GetAllProductPaginationAsync(int pageNumber, int pageSize);
      
        Task DeleteProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task SaveAsync();
    }
}
