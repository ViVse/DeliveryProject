using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class CouponService: CouponProtoService.CouponProtoServiceBase
    {
        private readonly ICouponRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponService> _logger;

        public CouponService(ICouponRepository repository, IMapper mapper, ILogger<CouponService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetCoupon(request.Code);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Code={request.Code} was not found"));
            }
            _logger.LogInformation("Coupon is retrived for Code: {code}, isPercent: {isPercent}, Amount: {amount}", coupon.Code, coupon.IsPercent, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.CreateCoupon(coupon);
            _logger.LogInformation("Coupon was created. Code: {Code}", coupon.Code);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.UpdateCoupon(coupon);
            _logger.LogInformation("Coupon is successfully updated. Code: {Code}", coupon.Code);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteCoupon(request.Id);
            var response = new DeleteCouponResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
