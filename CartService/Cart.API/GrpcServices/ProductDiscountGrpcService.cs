using Discount.Grpc.Protos;

namespace Cart.API.GrpcServices
{
    public class ProductDiscountGrpcService
    {
        private readonly ProductDiscountProtoService.ProductDiscountProtoServiceClient _discountProtoService;

        public ProductDiscountGrpcService(ProductDiscountProtoService.ProductDiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        public async Task<ProductDiscountModel> GetDiscount(int productId)
        {
            var discountRequest = new GetDiscountRequest { ProductId = productId };
            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}
