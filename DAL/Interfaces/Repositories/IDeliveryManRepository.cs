using DAL.Entities;

namespace DAL.Interfaces.Repositories
{
    public interface IDeliveryManRepository
    {
        Task<IEnumerable<DeliveryMan>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<DeliveryMan> GetByIdAsync(int id);
        Task InsertAsync(DeliveryMan t);
        Task UpdateAsync(DeliveryMan t);
    }
}
