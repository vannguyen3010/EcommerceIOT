using Entities.Models;

namespace Contracts
{
    public interface ICateProductsRepository
    {
        Task<CateProduct> GetCategoryByNameAsync(string name);
        Task<CateProduct> CreateCategoryAsync(CateProduct cateProducs);
        Task<IEnumerable<CateProduct>> GetAllCategoryProduct(bool trackChanges);
        Task<CateProduct> GetCategoryProductByIdAsync(Guid id, bool trackChanges);
        Task UpdateCategoryAsync(CateProduct cateProduct);
        Task<bool> HasProductsInCategoryAsync(Guid categoryId);
        Task DeleteCategoryAsync(CateProduct category);

    }
}
