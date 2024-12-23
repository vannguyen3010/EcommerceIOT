﻿namespace Contracts
{
    public interface IRepositoryManager
    {
        IContactRepository Contact { get; }
        IProfileRepository Profile { get; }
        ICateIntroduceRepository CateIntroduce { get; }
        IIntroduceRepository Introduce { get; }
        ICateProductsRepository CateProduct { get; }
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        IOrderRepository Order { get; }
        void SaveAsync();
    }
}
