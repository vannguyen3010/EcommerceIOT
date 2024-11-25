namespace Contracts
{
    public interface IRepositoryManager
    {
        IContactRepository Contact { get; }
        IProfileRepository Profile { get; }
        void SaveAsync();
    }
}
