using DAL.Entities;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        /*SqlConnection _sqlConnection;
        IDbTransaction _dbTransaction;*/
        protected readonly Context _context;
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public IAddressRepository AddressRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IShopRepository ShopRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IDeliveryManRepository DeliveryManRepository { get; }

        public UnitOfWork(
            /*SqlConnection sqlConnection,
            IDbTransaction dbTransaction,*/
            Context context, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IAddressRepository addressRepository,
            IProductRepository productRepository, 
            IShopRepository shopRepository,
            IReviewRepository reviewRepository,
            IDeliveryManRepository deliveryManRepository)
        {
           /* _sqlConnection = sqlConnection;
            _dbTransaction = dbTransaction;*/
            _context = context;
            UserManager = userManager;
            SignInManager = signInManager;
            AddressRepository = addressRepository;
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
            ReviewRepository = reviewRepository;
            DeliveryManRepository = deliveryManRepository;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
/*            try
            {
               _dbTransaction.Commit();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _dbTransaction.Rollback();
            }*/
        }

        public async Task Dispose()
        {
            await _context.DisposeAsync();
            /*_dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();*/
        }
    }
}
