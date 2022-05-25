using DAL.Entities;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DAL.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        protected readonly Context _context;
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public IAddressRepository AddressRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IShopRepository ShopRepository { get; }

        public UnitOfWork(Context context, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IAddressRepository addressRepository, 
            IProductRepository productRepository, 
            IShopRepository shopRepository)
        {
            _context = context;
            UserManager = userManager;
            SignInManager = signInManager;
            AddressRepository = addressRepository;
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
