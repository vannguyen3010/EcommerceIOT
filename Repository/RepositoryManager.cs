using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repoContext;
        private IContactRepository _contact;
        private IProfileRepository _ProfileInfo;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IContactRepository Contact
        {
            get
            {
                if (_contact == null)
                {
                    _contact = new ContactRepository(_repoContext);
                }

                return _contact;
            }
        }

        public IProfileRepository Profile
        {
            get
            {
                if (_ProfileInfo == null)
                {
                    _ProfileInfo = new ProfileRepository(_repoContext);
                }

                return _ProfileInfo;
            }
        }

        public void SaveAsync() => _repoContext.SaveChanges();
    }
}
