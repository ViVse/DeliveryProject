using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class AddressService: IAddressService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AddressResponse>> GetAsync()
        {
            var addresses = await unitOfWork.AddressRepository.GetAsync();
            return addresses?.Select(mapper.Map<Address, AddressResponse>);
        }

        public async Task<AddressResponse> GetByIdAsync(int id)
        {
            var address = await unitOfWork.AddressRepository.GetByIdAsync(id);
            return mapper.Map<Address, AddressResponse>(address);
        }

        public async Task InsertAsync(AddressRequest request)
        {
            var address = mapper.Map<AddressRequest, Address>(request);
            await unitOfWork.AddressRepository.InsertAsync(address);
            await unitOfWork.Commit();
        }

        public async Task UpdateAsync(AddressRequest request)
        {
            var address = mapper.Map<AddressRequest, Address>(request);
            await unitOfWork.AddressRepository.UpdateAsync(address);
            await unitOfWork.Commit();
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.AddressRepository.DeleteAsync(id);
            await unitOfWork.Commit();
        }
    }
}
