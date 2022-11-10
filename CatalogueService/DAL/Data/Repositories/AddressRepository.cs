using DAL.Entities;
using DAL.Interfaces.Repositories;

namespace DAL.Data.Repositories
{
    public class AddressRepository: GenericRepository<Address>, IAddressRepository
    {
        Context context;

        public AddressRepository(Context context): base(context) { 
            this.context = context;
        }

        public override async Task<int> InsertAsync(Address entity) {
            await table.AddAsync(entity);
            context.SaveChanges();
            return entity.Id;
        }
       
    }
}
