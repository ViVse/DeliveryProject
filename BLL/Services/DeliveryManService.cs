using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class DeliveryManService : IDeliveryManService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DeliveryManService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.DeliveryManRepository.DeleteAsync(id);
            await unitOfWork.Commit();
        }

        public async Task<IEnumerable<DeliveryManResponse>> GetAsync()
        {
            var deliveryMen = await unitOfWork.DeliveryManRepository.GetAsync();
            return deliveryMen?.Select(mapper.Map<DeliveryMan, DeliveryManResponse>);
        }

        public async Task<DeliveryManResponse> GetByIdAsync(int id)
        {
            var deliveryMan = await unitOfWork.DeliveryManRepository.GetByIdAsync(id);
            return mapper.Map<DeliveryMan, DeliveryManResponse>(deliveryMan);
        }

        public async Task InsertAsync(DeliveryManRequest request)
        {
            var deliveryMan = mapper.Map<DeliveryManRequest, DeliveryMan>(request);
            await unitOfWork.DeliveryManRepository.InsertAsync(deliveryMan);
            await unitOfWork.Commit();
        }

        public async Task UpdateAsync(DeliveryManRequest request)
        {
            var deliveryMan = mapper.Map<DeliveryManRequest, DeliveryMan>(request);
            await unitOfWork.DeliveryManRepository.UpdateAsync(deliveryMan);
            await unitOfWork.Commit();
        }
    }
}
