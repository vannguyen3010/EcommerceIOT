using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CateIntroduceRepository : RepositoryBase<CategoryNew>, ICateIntroduceRepository
    {
        private readonly RepositoryContext _dbContext;

        public CateIntroduceRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CategoryNew> CreateCategoryIntroduceAsync(CategoryNew categoryIntroduce, bool trackChanges)
        {
            await _dbContext.CategoryNews.AddAsync(categoryIntroduce);
            await SaveAsync();
            return categoryIntroduce;
        }

        public void DeleteCategory(CategoryNew categoryIntroduce)
        {
            Delete(categoryIntroduce);
        }


        public async Task<CategoryNew> GetCategoryIntroduceByIdAsync(Guid categoryId, bool trackChanges)
        {
            return await FindByCondition(category => category.Id.Equals(categoryId), trackChanges)
           .FirstOrDefaultAsync();
        }

        public void UpdateCategory(CategoryNew categoryIntroduce)
        {
            Update(categoryIntroduce);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CategoryNew> GetCategoryIntroduceByNameAsync(string name)
        {
            var lowerName = name.ToLower(); // Chuyển đổi tên danh mục về dạng chữ thường

            return await _dbContext.CategoryNews
              .Where(x => x.Name.ToLower() == lowerName) // So sánh chuỗi sau khi chuyển đổi về dạng chữ thường
              .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<CategoryNew> CategoryIntroduce, int Total)> GetAllCategoryIntroducePagitionAsync(int pageNumber, int pageSize, string? keyword = null)
        {
            var categoryQuery = _dbContext.CategoryNews.AsQueryable();

            //Lọc theo keyword nếu có
            if (!string.IsNullOrEmpty(keyword))
            {
                string lowerCaseName = keyword.ToLower();

                categoryQuery = categoryQuery.Where(x => x.Name.ToLower().Contains(lowerCaseName));
            }

            int totalCount = await categoryQuery.CountAsync();

            //Phân trang
            var categories = await categoryQuery
                .Include(x => x.New)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (categories, totalCount);
        }

        public async Task<IEnumerable<CategoryNew>> GetAllCategoryIntroduceAsync(bool trackChanges)
        {
            return await _dbContext.CategoryNews
                    .Include(x => x.New)
                    .OrderBy(x => x.Id)
                    .ToListAsync();
        }
    }
}
