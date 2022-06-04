using DAL.Entities;

namespace DAL.Interfaces.Repositories
{
    public interface IDeliveryManRepository
    {
        Task<IEnumerable<DeliveryMan>> GetAll();
        Task Delete(int id);
        Task<DeliveryMan> GetById(int id);
        Task Insert(DeliveryMan t);
        Task Update(DeliveryMan t);
    }
}
