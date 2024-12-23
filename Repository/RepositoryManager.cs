﻿using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repoContext;
        private IContactRepository _contact;
        private IProfileRepository _ProfileInfo;
        private ICateIntroduceRepository _categoryIntroduce;
        private IIntroduceRepository _introduceRepository;
        private ICateProductsRepository _cateProductsRepository;
        private IProductRepository _productRepository;
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;
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

        public ICateProductsRepository CateProduct
        {
            get
            {
                if (_cateProductsRepository == null)
                {
                    _cateProductsRepository = new CateProductsRepository(_repoContext);
                }

                return _cateProductsRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_repoContext);
                }

                return _productRepository;
            }
        }

        public ICartRepository Cart
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_repoContext);
                }

                return _cartRepository;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_repoContext);
                }

                return _orderRepository;
            }
        }

        public void SaveAsync() => _repoContext.SaveChanges();
    }
}
