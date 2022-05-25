using DAL.Entities;
using DAL.Interfaces.Repositories;
using DAL.Pagination;
using DAL.Parameters;

namespace DAL.Data.Repositories
{
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(Context context): base(context) { }

        public async Task<PagedList<Product>> GetAsync(ProductParameters parameters)
        {
            var products = FindByCondition(o => 
                                            o.Price >= parameters.MinPrice &&
                                            (o.Price <= parameters.MaxPrice || parameters.MaxPrice == null) &&
                                            o.ProductionTime >= parameters.MinProductionTime &&
                                            (o.ProductionTime <= parameters.MaxProductionTime || parameters.MaxProductionTime == null));

            SearchByName(ref products, parameters.Name);

            return await PagedList<Product>.ToPagedListAsync(products, parameters.PageNumber, parameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Product> products, string name)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(name))
                return;

            products = products.Where(o => o.Name.ToLower().Contains(name.Trim().ToLower()));
        }
    }
}
