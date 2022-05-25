using DAL.Entities;
using DAL.Interfaces.Repositories;

namespace DAL.Data.Repositories
{
    public class AddressRepository: GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(Context context): base(context) { }
    }
}
