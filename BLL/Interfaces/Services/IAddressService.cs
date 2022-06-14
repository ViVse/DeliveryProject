using BLL.DTO.Requests;
using BLL.DTO.Responses;

namespace BLL.Interfaces.Services
{
    public interface IAddressService
    {

        Task<IEnumerable<AddressResponse>> GetAsync();
        
        Task<AddressResponse> GetByIdAsync(int id);
        
        Task<int> InsertAsync(AddressRequest request);

        Task UpdateAsync(AddressRequest request);

        Task DeleteAsync(int id);
    }
}
