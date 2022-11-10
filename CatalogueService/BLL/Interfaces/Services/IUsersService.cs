using BLL.DTO.Requests;
using BLL.DTO.Responses;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Interfaces.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserResponse>> GetAsync();

        Task<PagedList<UserResponse>> GetAsync(UserParameters parameters);

        Task<UserResponse> GetByIdAsync(string id);

        Task UpdateAsync(UserRequest request);

        Task DeleteAsync(string id);
    }
}
