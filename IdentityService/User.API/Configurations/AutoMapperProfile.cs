using AutoMapper;
using User.API.DTO.Request;
using User.API.Data;
using User.API.DTO.Response;

namespace User.API.Configurations
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignUpRequest, UserModel>();
            CreateMap<UserRequest, UserModel>();
            CreateMap<UserModel, UserResponse>();
        }
    }
}
