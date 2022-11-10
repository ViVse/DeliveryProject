using DAL.Entities;

namespace DAL.Interfaces.Repositories
{
    public interface IAddressRepository: IRepository<Address>
    {
        public Task<int> InsertAsync(Address entity);
    }
}
