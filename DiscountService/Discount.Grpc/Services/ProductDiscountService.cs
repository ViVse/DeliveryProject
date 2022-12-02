using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Interfaces;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class ProductDiscountService : ProductDiscountProtoService.ProductDiscountProtoServiceBase
    {
        private readonly IProductDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductDiscountService> _logger;

        public ProductDiscountService(IProductDiscountRepository repository, IMapper mapper, ILogger<ProductDiscountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<ProductDiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var discount = await _repository.GetDiscount(request.ProductId);
            if (discount == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductId={request.ProductId} was not found"));
            }
            _logger.LogInformation("Discount is retrived for ProductId: {productId}, isPercent: {isPercent}, Amount: {amount}", discount.ProductId, discount.IsPercent, discount.Amount);

            var discountModel = _mapper.Map<ProductDiscountModel>(discount);
            return discountModel;
        }

        public override async Task<ProductDiscountModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var discount = _mapper.Map<ProductDiscount>(request.Discount);

            await _repository.CreateDiscount(discount);
            _logger.LogInformation("Discount was created. ProductId: {ProductId}", discount.ProductId);

            var discountModel = _mapper.Map<ProductDiscountModel>(discount);
            return discountModel;
        }

        public override async Task<ProductDiscountModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var discount = _mapper.Map<ProductDiscount>(request.Discount);

            await _repository.UpdateDiscount(discount);
            _logger.LogInformation("Discount is successfully updated. ProductId: {ProductId}", discount.ProductId);

            var discountModel = _mapper.Map<ProductDiscountModel>(discount);
            return discountModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.Id);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
