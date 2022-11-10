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
    public class ProductService: IProductService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponse>> GetAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAsync();
            return products?.Select(mapper.Map<Product, ProductResponse>);
        }

        public async Task<PagedList<ProductResponse>> GetAsync(ProductParameters parameters)
        {
            var productPage = await unitOfWork.ProductRepository.GetAsync(parameters);
            return productPage?.Map(mapper.Map<Product, ProductResponse>);
        }

        public async Task<ProductResponse> GetByIdAsync(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            return mapper.Map<Product, ProductResponse>(product);
        }

        public async Task InsertAsync(ProductRequest request)
        {
            var product = mapper.Map<ProductRequest, Product>(request);
            await unitOfWork.ProductRepository.InsertAsync(product);
            await unitOfWork.Commit();
        }

        public async Task UpdateAsync(ProductRequest request)
        {
            var product = mapper.Map<ProductRequest, Product>(request);
            await unitOfWork.ProductRepository.UpdateAsync(product);
            await unitOfWork.Commit();
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.ProductRepository.DeleteAsync(id);
            await unitOfWork.Commit();
        }
    }
}
