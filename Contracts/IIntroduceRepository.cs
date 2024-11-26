using Entities.Models;

namespace Contracts
{
    public interface IIntroduceRepository
    {
        Task<New> GetIntroduceByNameAsync(string name);
        Task<New> CreateIntroduceAsync(New introduce);
        Task<(IEnumerable<New> Introduces, int Total)> GetListIntroduceAsync(int pageNumber, int pageSize, Guid? categoryId = null, string? keyword = null, int? type = null);
        Task<IEnumerable<New>> GetAllIntroduceIsHotAsync();
        Task<New> GetIntroduceByIdAsync(Guid introduceId, bool trackChanges);
        Task<IEnumerable<New>> GetRelatedIntroducesAsync(Guid introduceId, Guid categoryId, bool trackChanges);
        void UpdateIntroduce(New introduce);
        void DeleteIntroduce(New introduce);
    }
}
