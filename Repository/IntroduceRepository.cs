using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class IntroduceRepository : RepositoryBase<New>, IIntroduceRepository
    {
        private readonly RepositoryContext _dbContext;

        public IntroduceRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<New> CreateIntroduceAsync(New introduce)
        {
            await _dbContext.News.AddAsync(introduce);
            await _dbContext.SaveChangesAsync();
            return introduce;
        }

        public void DeleteIntroduce(New introduce)
        {
            Delete(introduce);
        }

        public async Task<IEnumerable<New>> GetAllIntroduceIsHotAsync()
        {
            return await _dbContext.News.Where(x => x.IsHot).ToListAsync();
        }

        public async Task<New> GetIntroduceByIdAsync(Guid introduceId, bool trackChanges)
        {
            //return await FindByCondition(introduce => introduce.id.Equals(introduceId), trackChanges).FirstOrDefaultAsync();
            if (trackChanges)
            {
                return await _dbContext.News
                    .Include(x => x.CategoryNew)
                    .FirstOrDefaultAsync(x => x.Id == introduceId);
            }
            else
            {
                return await _dbContext.News
                 .AsNoTracking()
                .Include(x => x.CategoryNew)
                .FirstOrDefaultAsync(x => x.Id == introduceId);
            }
        }

        public async Task<New> GetIntroduceByNameAsync(string name)
        {
            var lowerName = name.ToLower(); // Chuyển đổi tên danh mục về dạng chữ thường

            return await _dbContext.News
              .Where(x => x.Name.ToLower() == lowerName) // So sánh chuỗi sau khi chuyển đổi về dạng chữ thường
              .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<New> Introduces, int Total)> GetListIntroduceAsync(int pageNumber, int pageSize, Guid? categoryId = null, string? keyword = null, int? type = null)
        {
            var introducesQuery = _dbContext.News.AsQueryable();

            if (type.HasValue)
            {
                if (type == 3)
                {
                    introducesQuery = introducesQuery.Where(x => x.IsHot == true);
                }
            }

            //Lọc bài viết theo danh mục
            if (categoryId.HasValue)
            {
                introducesQuery = introducesQuery.Where(x => x.CategoryId == categoryId.Value);
            }

            //Lọc theo keyword nếu có
            if (!string.IsNullOrEmpty(keyword))
            {
                string lowerCaseName = keyword.ToLower();

                introducesQuery = introducesQuery.Where(x => x.Name.ToLower().Contains(lowerCaseName));
            }

            int totalCount = await introducesQuery.CountAsync();

            //Phân trang
            var introduces = await introducesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (introduces, totalCount);
        }

        public async Task<IEnumerable<New>> GetRelatedIntroducesAsync(Guid introduceId, Guid categoryId, bool trackChanges)
        {
            return await _dbContext.News
                .Where(x => x.CategoryId == categoryId && x.Id != introduceId)
                .ToListAsync();
        }

        public void UpdateIntroduce(New introduce)
        {
            Update(introduce);
        }
    }
}
