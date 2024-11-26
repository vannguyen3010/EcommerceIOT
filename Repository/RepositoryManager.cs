using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repoContext;
        private IContactRepository _contact;
        private IProfileRepository _ProfileInfo;
        private ICateIntroduceRepository _categoryIntroduce;
        private IIntroduceRepository _introduceRepository;
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

        public ICateIntroduceRepository CateIntroduce
        {
            get
            {
                if (_categoryIntroduce == null)
                {
                    _categoryIntroduce = new CateIntroduceRepository(_repoContext);
                }

                return _categoryIntroduce;
            }
        }

        public IIntroduceRepository Introduce
        {
            get
            {
                if (_introduceRepository == null)
                {
                    _introduceRepository = new IntroduceRepository(_repoContext);
                }

                return _introduceRepository;
            }
        }


        public void SaveAsync() => _repoContext.SaveChanges();
    }
}
