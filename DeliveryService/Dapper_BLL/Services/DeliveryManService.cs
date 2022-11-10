using AutoMapper;
using Dapper_BLL.DTO.Requests;
using Dapper_BLL.DTO.Responses;
using Dapper_BLL.Interfaces;
using Dapper_DAL.Entites;
using Dapper_DAL.Interfaces;

namespace Dapper_BLL.Services
{
    public class DeliveryManService: IDeliveryManService
    {
        private readonly IDeliveryManRepository deliveryManRepository;
        private readonly IMapper mapper;

        public DeliveryManService(IDeliveryManRepository deliveryManRepository, IMapper mapper)
        {
            this.deliveryManRepository = deliveryManRepository;
            this.mapper = mapper;
        }

        public async Task DeleteAsync(int id)
        {
            await deliveryManRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DeliveryManResponse>> GetAsync()
        {
            var deliveryMen = await deliveryManRepository.GetAsync();
            return deliveryMen?.Select(mapper.Map<DeliveryMan, DeliveryManResponse>);
        }

        public async Task<DeliveryManResponse> GetByIdAsync(int id)
        {
            var deliveryMan = await deliveryManRepository.GetByIdAsync(id);
            return mapper.Map<DeliveryMan, DeliveryManResponse>(deliveryMan);
        }

        public async Task InsertAsync(DeliveryManRequest request)
        {
            var deliveryMan = mapper.Map<DeliveryManRequest, DeliveryMan>(request);
            await deliveryManRepository.InsertAsync(deliveryMan);
        }

        public async Task UpdateAsync(DeliveryManRequest request)
        {
            var deliveryMan = mapper.Map<DeliveryManRequest, DeliveryMan>(request);
            await deliveryManRepository.UpdateAsync(deliveryMan);
        }
    }
}
