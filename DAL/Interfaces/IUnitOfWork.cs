using DAL.Entities;
using DAL.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        UserManager<User> UserManager { get; }
        SignInManager<User> SignInManager { get; }
        IAddressRepository AddressRepository { get; }
        IProductRepository ProductRepository { get; }
        IShopRepository ShopRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IDeliveryManRepository DeliveryManRepository { get; }

        Task Commit();
        Task Dispose();
    }
}
