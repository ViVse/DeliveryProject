using Dapper_BLL.DTO.Requests;
using Dapper_BLL.DTO.Responses;

namespace Dapper_BLL.Interfaces
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
