using BLL.DTO.Requests;
using BLL.DTO.Responses;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Interfaces.Services
{
    public interface IProductService
    {

        Task<IEnumerable<ProductResponse>> GetAsync();

        Task<PagedList<ProductResponse>> GetAsync(ProductParameters parameters);

        Task<ProductResponse> GetByIdAsync(int id);

        Task InsertAsync(ProductRequest request);

        Task UpdateAsync(ProductRequest request);

        Task DeleteAsync(int id);

    }
}
