using BLL.DTO.Requests;
using BLL.DTO.Responses;

namespace BLL.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<JwtResponse> SignInAsync(UserSignInRequest request);

        Task<JwtResponse> SignUpAsync(UserSignUpRequest request);
    }
}
