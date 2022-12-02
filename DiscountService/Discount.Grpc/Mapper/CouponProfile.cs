using AutoMapper;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class CouponProfile: Profile
    {
        public CouponProfile()
        {
            CreateMap<CouponProfile, CouponModel>().ReverseMap();
        }
    }
}
