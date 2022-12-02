using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
            CreateMap<ProductDiscount, ProductDiscountModel>().ReverseMap();
        }
    }
}
