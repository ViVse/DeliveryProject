using User.API.DTO.Request;
using User.API.DTO.Response;

namespace User.API.Interfaces
{
    public interface IUserService
    {
        Task<JwtResponse> SignInAsync(SignInRequest request);

        Task<JwtResponse> SignUpAsync(SignUpRequest request);

        Task<IEnumerable<UserResponse>> GetAsync();

        Task<UserResponse> GetByIdAsync(string id);

        Task UpdateAsync(UserRequest request);

        Task DeleteAsync(string id);
    }
}
