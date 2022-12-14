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
            CreateProductMaps();
            CreateShopMaps();
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
