using DAL.Entities;
using DAL.Pagination;
using DAL.Parameters;

namespace DAL.Interfaces.Repositories
{
    public interface IShopRepository: IRepository<Shop>
    {
        public Task<PagedList<Shop>> GetAsync(ShopParameters parameters);
    }
}
