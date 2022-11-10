using AutoMapper;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Services
{
    public class ShopService: IShopService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ShopService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ShopResponse>> GetAsync()
        {
            var shops = await unitOfWork.ShopRepository.GetAsync();
            return shops?.Select(mapper.Map<Shop, ShopResponse>);
        }

        public async Task<PagedList<ShopResponse>> GetAsync(ShopParameters parameters)
        {
            var productPage = await unitOfWork.ShopRepository.GetAsync(parameters);
            return productPage?.Map(mapper.Map<Shop, ShopResponse>);
        }

        public async Task<ShopResponse> GetByIdAsync(int id)
        {
            var shop = await unitOfWork.ShopRepository.GetByIdAsync(id);
            return mapper.Map<Shop, ShopResponse>(shop);
        }

        public async Task InsertAsync(ShopRequest request)
        {
            var shop = mapper.Map<ShopRequest, Shop>(request);
            await unitOfWork.ShopRepository.InsertAsync(shop);
            await unitOfWork.Commit();
        }

        public async Task UpdateAsync(ShopRequest request)
        {
            var shop = mapper.Map<ShopRequest, Shop>(request);
            await unitOfWork.ShopRepository.UpdateAsync(shop);
            await unitOfWork.Commit();
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.ShopRepository.DeleteAsync(id);
            await unitOfWork.Commit();
        }

    }
}
