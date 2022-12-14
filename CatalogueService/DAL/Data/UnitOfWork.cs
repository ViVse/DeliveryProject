using DAL.Interfaces;
using DAL.Interfaces.Repositories;

namespace DAL.Data
{
    public class UnitOfWork: IUnitOfWork
    {
       
        protected readonly Context _context;
        public IAddressRepository AddressRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IShopRepository ShopRepository { get; }

        public UnitOfWork(
            Context context, 
            IAddressRepository addressRepository,
            IProductRepository productRepository, 
            IShopRepository shopRepository)
        {
            _context = context;
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
