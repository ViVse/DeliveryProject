using AutoMapper;
using IdentityServer.Data;
using IdentityServer.DTO.Request;
using IdentityServer.DTO.Response;

namespace IdentityServer.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignUpRequest, UserModel>();
            CreateMap<UserRequest, UserModel>();
            CreateMap<UserModel, UserResponse>();
        }
    }
}
