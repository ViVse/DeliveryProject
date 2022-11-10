using BLL.DTO.Requests;
using BLL.DTO.Responses;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Interfaces.Services
{
    public interface IShopService
    {

        Task<IEnumerable<ShopResponse>> GetAsync();

        Task<PagedList<ShopResponse>> GetAsync(ShopParameters parameters);

        Task<ShopResponse> GetByIdAsync(int id);

        Task InsertAsync(ShopRequest request);

        Task UpdateAsync(ShopRequest request);

        Task DeleteAsync(int id);

    }
}
