using DAL.Entities;
using DAL.Pagination;
using DAL.Parameters;

namespace DAL.Interfaces.Repositories
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<PagedList<Product>> GetAsync(ProductParameters parameters);
    }
}
