using DAL.Entities;
using DAL.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        IAddressRepository AddressRepository { get; }
        IProductRepository ProductRepository { get; }
        IShopRepository ShopRepository { get; }

        Task Commit();
        Task Dispose();
    }
}
