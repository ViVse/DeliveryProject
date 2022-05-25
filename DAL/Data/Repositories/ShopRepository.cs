using DAL.Entities;
using DAL.Exceptions;
using DAL.Interfaces.Repositories;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories
{
    public class ShopRepository: GenericRepository<Shop>, IShopRepository
    {
        public ShopRepository(Context context): base(context) { }

        public override async Task<IEnumerable<Shop>> GetAsync() => await table.Include(s => s.Address).ToListAsync();

        public override async Task<Shop> GetByIdAsync(int id)
        {
            return await table.Include(s => s.Address).FirstOrDefaultAsync(s => s.Id == id) ?? throw new EntityNotFoundException(GetEntityNotFoundErrorMeassage(id));
        }

        public async Task<PagedList<Shop>> GetAsync(ShopParameters parameters)
        {
            IQueryable<Shop> source = table.Include(s => s.Address);

            SearchByName(ref source, parameters.Name);
            SearchByCity(ref source, parameters.City);

            return await PagedList<Shop>.ToPagedListAsync(source, parameters.PageNumber, parameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Shop> shops, string name)
        {
            if (!shops.Any() || string.IsNullOrEmpty(name))
                return;

            shops = shops.Where(s => s.Name.ToLower().Contains(name.Trim().ToLower()));
        }

        private void SearchByCity(ref IQueryable<Shop> shops, string city)
        {
            if (!shops.Any() || string.IsNullOrEmpty(city))
                return;

            shops = shops.Where(s => s.Address.City.ToLower().Contains(city.Trim().ToLower()));
        }
    }
}
