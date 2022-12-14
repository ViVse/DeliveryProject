using DAL.Interfaces.Repositories;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IAddressRepository AddressRepository { get; }
        IProductRepository ProductRepository { get; }
        IShopRepository ShopRepository { get; }

        Task Commit();
        Task Dispose();
    }
}
