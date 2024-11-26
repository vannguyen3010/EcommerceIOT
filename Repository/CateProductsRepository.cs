using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CateProductsRepository : RepositoryBase<CateProduct>, ICateProductsRepository
    {
        private readonly RepositoryContext _dbContext;

        public CateProductsRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CateProduct> CreateCategoryAsync(CateProduct cateProducs)
        {
            await _dbContext.CateProducts.AddAsync(cateProducs);
            await _dbContext.SaveChangesAsync();
            return cateProducs;
        }

        public async Task DeleteCategoryAsync(CateProduct category)
        {
            _dbContext.CateProducts.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CateProduct>> GetAllCategoryProduct(bool trackChanges)
        {
            return await _dbContext.CateProducts
                .Include(x => x.Products)
                .ToListAsync();
        }

        public async Task<CateProduct> GetCategoryByNameAsync(string name)
        {
            var lowerName = name.ToLower();
            return await _dbContext.CateProducts
                .Where(c => c.Name.ToLower() == lowerName)
                .FirstOrDefaultAsync();
        }

        public async Task<CateProduct> GetCategoryProductByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }

        public async Task<bool> HasProductsInCategoryAsync(Guid categoryId)
        {
            return await _dbContext.Products.AnyAsync(x => x.CategoryId == categoryId);
        }

        public async Task UpdateCategoryAsync(CateProduct cateProduct)
        {
            _dbContext.CateProducts.Update(cateProduct);
            await _dbContext.SaveChangesAsync();
        }
    }
}
