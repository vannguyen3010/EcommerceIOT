using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProfileRepository : RepositoryBase<ProfileInfo>, IProfileRepository
    {
        private readonly RepositoryContext _dbContext;

        public ProfileRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProfileInfo> GetSocialMediaInfoAsync(Guid id, bool trackChanges)
        {
            return await _dbContext.ProfileInfos.FindAsync(id);
        }

        public async Task CreateSocialMediaInfoAsync(ProfileInfo profile)
        {
            await _dbContext.ProfileInfos.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateSocialMediaInfoAsync(ProfileInfo profile)
        {
            _dbContext.ProfileInfos.Update(profile);
            await _dbContext.SaveChangesAsync();
        }
    }
}
