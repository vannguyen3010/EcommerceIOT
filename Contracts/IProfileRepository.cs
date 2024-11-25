using Entities.Models;

namespace Contracts
{
    public interface IProfileRepository
    {
        Task<ProfileInfo> GetSocialMediaInfoAsync(Guid id, bool trackChanges);
        Task CreateSocialMediaInfoAsync(ProfileInfo profile);
        Task UpdateSocialMediaInfoAsync(ProfileInfo profile);
    }
}
