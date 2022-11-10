using AutoMapper;
using Dapper_BLL.DTO.Requests;
using Dapper_BLL.DTO.Responses;
using Dapper_DAL.Entites;

namespace Dapper_BLL.Configurations
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateDeliveryManMaps();
            CreateReviewMaps();
        }

        private void CreateDeliveryManMaps()
        {
            CreateMap<DeliveryManRequest, DeliveryMan>();
            CreateMap<DeliveryMan, DeliveryManResponse>();
        }

        private void CreateReviewMaps()
        {
            CreateMap<ReviewRequest, Review>();
            CreateMap<Review, ReviewResponse>();
        }
    }
}
