using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using DAL.Entities;

namespace BLL.Configurations
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateAddressMaps();
            CreateUsersMaps();
            CreateProductMaps();
            CreateShopMaps();
        }

        private void CreateUsersMaps()
        {
            CreateMap<UserSignUpRequest, User>();
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>()
                .ForMember(response => response.FullName,
                options => options.MapFrom(user => $"{user.FirstName} {user.LastName}"));
        }

        private void CreateAddressMaps()
        {
            CreateMap<AddressRequest, Address>();
            CreateMap<Address, AddressResponse>();
        }

        private void CreateProductMaps()
        {
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
        }

        private void CreateShopMaps()
        {
            CreateMap<ShopRequest, Shop>();
            CreateMap<Shop, ShopResponse>()
                .ForMember(response => response.AddressLine, 
                options => options.MapFrom(address => address.Address.AddressLine))
                .ForMember(response => response.City,
                options => options.MapFrom(address => address.Address.City))
                 .ForMember(response => response.Latitude,
                options => options.MapFrom(address => address.Address.Latitude))
                  .ForMember(response => response.Longitude,
                options => options.MapFrom(address => address.Address.Longitude));
        }
    }
}
