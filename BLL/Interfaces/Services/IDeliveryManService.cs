using BLL.DTO.Requests;
using BLL.DTO.Responses;

namespace BLL.Interfaces.Services
{
    public interface IDeliveryManService
    {
        Task<IEnumerable<DeliveryManResponse>> GetAsync();

        //ToDo
        //Task<PagedList<DeliveryManResponse>> GetAsync(DeliveryManParameters parameters);

        Task<DeliveryManResponse> GetByIdAsync(int id);

        Task InsertAsync(DeliveryManRequest request);

        Task UpdateAsync(DeliveryManRequest request);

        Task DeleteAsync(int id);
    }
}