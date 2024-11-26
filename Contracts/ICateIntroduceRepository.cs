using Entities.Models;

namespace Contracts
{
    public interface ICateIntroduceRepository
    {
        Task<CategoryNew> CreateCategoryIntroduceAsync(CategoryNew categoryIntroduce, bool trackChanges);
        Task<CategoryNew> GetCategoryIntroduceByIdAsync(Guid categoryId, bool trackChanges);
        void UpdateCategory(CategoryNew categoryIntroduce);
        void DeleteCategory(CategoryNew categoryIntroduce);
        Task<CategoryNew> GetCategoryIntroduceByNameAsync(string name);
        Task<(IEnumerable<CategoryNew> CategoryIntroduce, int Total)> GetAllCategoryIntroducePagitionAsync(int pageNumber, int pageSize, string? keyword = null);
        Task SaveAsync();
    }
}
