using IdentityServer.DTO.Request;
using IdentityServer.DTO.Response;

namespace IdentityServer.Services
{
    public interface IUserService
    {
        Task SignUpAsync(SignUpRequest request);

        Task<IEnumerable<UserResponse>> GetAsync();

        Task<UserResponse> GetByIdAsync(string id);

        Task UpdateAsync(UserRequest request);

        Task DeleteAsync(string id);
    }
}
