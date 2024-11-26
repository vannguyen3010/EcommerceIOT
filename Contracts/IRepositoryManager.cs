namespace Contracts
{
    public interface IRepositoryManager
    {
        IContactRepository Contact { get; }
        IProfileRepository Profile { get; }
        ICateIntroduceRepository CateIntroduce { get; }
        IIntroduceRepository Introduce { get; }
        void SaveAsync();
    }
}
