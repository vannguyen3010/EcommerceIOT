namespace Contracts
{
    public interface IRepositoryManager
    {
        IContactRepository Contact { get; }
        void SaveAsync();
    }
}
